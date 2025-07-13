using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Extensions.Configuration;
using MusicServer.Models.Database;
using MusicServer.Models.Database.Commands;
using MusicServer.Services.Interfaces;

namespace MusicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadMusicAssetController : ControllerBase
    {
        const String folderName = "files";
        private readonly String folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        private readonly IConfiguration configuration;
        private readonly IUploadMusicService uploadMusicService;
        private readonly IUploadEncodeAndStreamFiles uploadEncodeAndStreamFiles;

        public UploadMusicAssetController(
            IConfiguration configuration,
            IUploadMusicService uploadMusicService,
            IUploadEncodeAndStreamFiles uploadEncodeAndStreamFiles)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            this.configuration = configuration;
            this.uploadMusicService = uploadMusicService;
            this.uploadEncodeAndStreamFiles = uploadEncodeAndStreamFiles;
        }

        [HttpPost("licence")]
        public async Task<IActionResult> PostLicence(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {

            try
            {
                var (blobFileName, uri) = await uploadMusicService.UploadFileToAzureBlob(myFile, ResourceTypes.Licence);
                var key = uploadMusicService.SaveAzureBlobInfoToDatabaseAndReturnKey(blobFileName, uri);
                //return CreatedAtRoute(routeName: "myFile", routeValues: new { fileName = key }, value: null);
                return Ok(new { licenceLink = uri });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("audio")]
        public async Task<IActionResult> PostAudio(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {

            try
            {
                var (blobFileName, uri) = await uploadMusicService.UploadFileToAzureBlob(myFile, ResourceTypes.Audio);
                var key = uploadMusicService.SaveAzureBlobInfoToDatabaseAndReturnKey(blobFileName, uri);
                //return CreatedAtRoute(routeName: "myFile", routeValues: new { fileName = key }, value: null);
                return Ok(new { audioLink = uri });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("DeleteAudio")]
        public IActionResult DeleteAudio(string blobName)
        {

            try
            {
                uploadMusicService.DeleteFileInAzureBlob(blobName, ResourceTypes.Audio);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("lyrics")]
        public async Task<IActionResult> PostLyrics(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {

            try
            {
                var (blobFileName, uri) = await uploadMusicService.UploadFileToAzureBlob(myFile, ResourceTypes.Lyrics);
                var key = uploadMusicService.SaveAzureBlobInfoToDatabaseAndReturnKey(blobFileName, uri);
                //return CreatedAtRoute(routeName: "myFile", routeValues: new { fileName = key }, value: null);
                return Ok(new { lyricsLink = uri });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("video")]
        public async Task<IActionResult> PostVideo(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {

            try
            {
                var (blobFileName, uri) = await uploadMusicService.UploadFileToAzureBlob(myFile, ResourceTypes.Video);
                var key = uploadMusicService.SaveAzureBlobInfoToDatabaseAndReturnKey(blobFileName, uri);
                return Ok(new { videoLink = uri });
                //return CreatedAtRoute(routeName: "myFile", routeValues: new { fileName = key }, value: null);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Use this API to get the URI of the file on Azure Blob Storage system or the file in download dialogue.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("{fileName}", Name = "myFile")]
        public IActionResult Get([FromRoute] string fileName)
        {
            var uri = uploadMusicService.GetFileUri(fileName);
            if (uri == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(uri);
            }
        }


        [HttpPost("EncryptFileMusic")]
        public IActionResult EncryptFileMusic([FromForm] CreateEncryptFileCommand command)
        {

            try
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(command.myFile.FileName);
                var fileExtension = System.IO.Path.GetExtension(command.myFile.FileName);

                string SavePath = Path.Combine(Directory.GetCurrentDirectory() + "/bin/file/", fileName+ fileExtension);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    command.myFile.CopyTo(stream);
                }
              

                string filePathNew = Directory.GetCurrentDirectory() + "/bin/keys/" + fileName + "1" + fileExtension;

                uploadMusicService.AES_Encrypt(SavePath, filePathNew, command.password);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("DecryptFileMusic")]
        public IActionResult DecryptFileMusic([FromForm] CreateEncryptFileCommand command)
        {

            try
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(command.myFile.FileName);
                var fileExtension = System.IO.Path.GetExtension(command.myFile.FileName);

                string filePath = Directory.GetCurrentDirectory() + "/bin/keys/" + fileName + fileExtension;

                string filePathNew = Directory.GetCurrentDirectory() + "/bin/keys/" + fileName + "2" + fileExtension;

                uploadMusicService.AES_Decrypt(filePath, filePathNew, command.password);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("CopyFileEncryptAndUploadAudio")]
        public IActionResult CopyFileEncryptAndUploadAudio([FromForm] DownloadEncryptFileCommand command)
        {

            try
            {
                uploadMusicService.CopyFileEncryptAndUploadToAzureBlob(command.blobName, command.password, ResourceTypes.Audio);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("CopyFileEncryptAndUploadAudioOwnerShip")]
        public IActionResult CopyFileEncryptAndUploadAudioOwnerShip([FromForm] DownloadEncryptFileOwnerShipCommand command)
        {

            try
            {
                var urlLink = uploadMusicService.CopyFileEncryptAndUploadToAzureBlobOwnerShip(command.blobName, command.old_password, command.new_password, ResourceTypes.Video);
                return Ok(new { musicLink = urlLink });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("DownloadFileEncryptAndUploadMedia")]
        public async Task<IActionResult> DownloadFileEncryptAndUploadMedia([FromForm] DownloadEncryptFileCommand command)
        {

            try
            {

                var currentDirectory = System.IO.Directory.GetCurrentDirectory();

                ConfigWrapper config = new ConfigWrapper(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(currentDirectory+"/appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build());

                try
                {
                    uploadMusicService.CopyFileEncryptAndUploadToAzureBlob(command.blobName, command.password, ResourceTypes.Video);
                    var urlStream = await uploadEncodeAndStreamFiles.RunAsync(config, command.blobName);
                    return Ok(new { url = urlStream });
                }
                catch (Exception exception)
                {
                    if (exception.Source.Contains("ActiveDirectory"))
                    {
                        Console.Error.WriteLine("TIP: Make sure that you have filled out the appsettings.json file before running this sample.");
                    }

                    Console.Error.WriteLine($"{exception.Message}");

                    ApiErrorException apiException = exception.GetBaseException() as ApiErrorException;
                    if (apiException != null)
                    {
                        Console.Error.WriteLine(
                            $"ERROR: API call failed with error code '{apiException.Body.Error.Code}' and message '{apiException.Body.Error.Message}'.");
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, apiException);
                }
                //return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("MediaTest")]
        public async Task<IActionResult> MediaTest(
            [FromForm(Name = "myFile")]IFormFile myFile)
        {
            ConfigWrapper config = new ConfigWrapper(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:/Users/ALIENWARE.000/Desktop/Front/MusicServer/MusicServer/appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build());

            var message = "";
            try
            {
                var urlStream = await uploadEncodeAndStreamFiles.RunAsync(config, "sample1-0c6fd45a-6cc0-4bb9-92d4-b4815b76612b.mp4");
                return Ok(new { url = urlStream });
            }
            catch (Exception exception)
            {
                if (exception.Source.Contains("ActiveDirectory"))
                {
                    Console.Error.WriteLine("TIP: Make sure that you have filled out the appsettings.json file before running this sample.");
                }

                Console.Error.WriteLine($"{exception.Message}");

                ApiErrorException apiException = exception.GetBaseException() as ApiErrorException;
                if (apiException != null)
                {
                    Console.Error.WriteLine(
                        $"ERROR: API call failed with error code '{apiException.Body.Error.Code}' and message '{apiException.Body.Error.Message}'.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, apiException);
            }


        }
    }
}
