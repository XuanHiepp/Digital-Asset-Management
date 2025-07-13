using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MusicServer.Models.Database;
using MusicServer.Repositories.Interfaces;
using MusicServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MusicServer.Services.Enforcements
{
    public class UploadMusicService : IUploadMusicService
    {
        private readonly IConfiguration configuration;
        private readonly string storageConnectionString;
        private readonly IMusicAssetRepository musicAssetRepository;

        public UploadMusicService(
            IConfiguration configuration,
            IMusicAssetRepository musicAssetRepository)
        {
            this.configuration = configuration;
            this.musicAssetRepository = musicAssetRepository;

            storageConnectionString = this.configuration["StorageBlobString"];
        }

        string IUploadMusicService.SaveAzureBlobInfoToDatabaseAndReturnKey(string blobName, string uri)
        {
            var blobResource = new MusicAsset()
            {
                Name = blobName,
                Uri = uri
            };
            var key = musicAssetRepository.CreateAndReturnKey(blobResource);
            return key;
        }

        /// <summary>
        /// Upload given file to Azure Blob Storage and return its unique name on Blob and the URI where the resource lives.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="resourceType"></param>
        /// <returns>
        ///     First return value is the unique file name of the file on Azure Blob Storage.
        ///     Second return value is the URI of the resource on Azure Blob Storage.
        /// </returns>
        async Task<(string, string)> IUploadMusicService.UploadFileToAzureBlob(IFormFile file, ResourceTypes resourceType)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            //Create a unique name for the container
            string containerName = "bug-container";
            switch (resourceType)
            {
                case (ResourceTypes.Audio):
                    {
                        containerName = "audio";
                        break;
                    }
                case (ResourceTypes.Lyrics):
                    {
                        containerName = "lyrics";
                        break;
                    }
                case (ResourceTypes.Video):
                    {
                        containerName = "video";
                        break;
                    }
                case (ResourceTypes.Licence):
                    {
                        containerName = "licence";
                        break;
                    }
                case (ResourceTypes.Other):
                default:
                    {
                        containerName = "other";
                        break;
                    }
            }


            // Create the container and return a container client object
            var response = await blobServiceClient.GetBlobContainerClient(containerName)
                .CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to a blob
            var fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = System.IO.Path.GetExtension(file.FileName);
            var uniqueFileName = fileName + "-" + Guid.NewGuid().ToString() + fileExtension;
            file.Headers["CONTENT-TYPE"] = this.GetFileContentType(file.FileName);
            var fileContentStream = file.OpenReadStream();
            BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);
            try
            {
                // Open the file and upload its data
                await blobClient.UploadAsync(fileContentStream);
                blobClient.SetHttpHeaders(
                    new Azure.Storage.Blobs.Models.BlobHttpHeaders()
                    {
                        ContentType = file.ContentType
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (uniqueFileName, blobClient.Uri.AbsoluteUri);
        }

        void IUploadMusicService.DeleteFileInAzureBlob(string blobName, ResourceTypes resourceType)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            //Create a unique name for the container
            string containerName = "bug-container";
            switch (resourceType)
            {
                case (ResourceTypes.Audio):
                    {
                        containerName = "audio";
                        break;
                    }
                case (ResourceTypes.Lyrics):
                    {
                        containerName = "lyrics";
                        break;
                    }
                case (ResourceTypes.Video):
                    {
                        containerName = "video";
                        break;
                    }
                case (ResourceTypes.Licence):
                    {
                        containerName = "licence";
                        break;
                    }
                case (ResourceTypes.Other):
                default:
                    {
                        containerName = "other";
                        break;
                    }
            }


            // Create the container and return a container client object
            var response = blobServiceClient.GetBlobContainerClient(containerName)
                .CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            blobClient.DeleteIfExistsAsync();
        }

        void IUploadMusicService.CopyFileEncryptAndUploadToAzureBlob(string blobName, string password, ResourceTypes resourceType = ResourceTypes.Other)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            //Create a unique name for the container
            string containerName = "bug-container";
            switch (resourceType)
            {
                case (ResourceTypes.Audio):
                    {
                        containerName = "audio";
                        break;
                    }
                case (ResourceTypes.Lyrics):
                    {
                        containerName = "lyrics";
                        break;
                    }
                case (ResourceTypes.Video):
                    {
                        containerName = "video";
                        break;
                    }
                case (ResourceTypes.Licence):
                    {
                        containerName = "licence";
                        break;
                    }
                case (ResourceTypes.Other):
                default:
                    {
                        containerName = "other";
                        break;
                    }
            }


            // Create the container and return a container client object
            var response = blobServiceClient.GetBlobContainerClient(containerName)
                .CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            string path_project_bin = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));

            
            using (var fileStream = System.IO.File.OpenWrite(path_project_bin + "/bin/file-test/test" + blobName))
            {
                blobClient.DownloadTo(fileStream);
                
            }

            (this as IUploadMusicService).AES_Decrypt(path_project_bin + "/bin/file-test/test" + blobName, path_project_bin + "/bin/file-test/test-decrypt" + blobName, password);
            File.Delete(path_project_bin + "/bin/file-test/test" + blobName);
        }

        string IUploadMusicService.CopyFileEncryptAndUploadToAzureBlobOwnerShip(string blobName, string old_password, string new_password, ResourceTypes resourceType = ResourceTypes.Other)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

            //Create a unique name for the container
            string containerName = "bug-container";
            switch (resourceType)
            {
                case (ResourceTypes.Audio):
                    {
                        containerName = "audio";
                        break;
                    }
                case (ResourceTypes.Lyrics):
                    {
                        containerName = "lyrics";
                        break;
                    }
                case (ResourceTypes.Video):
                    {
                        containerName = "video";
                        break;
                    }
                case (ResourceTypes.Licence):
                    {
                        containerName = "licence";
                        break;
                    }
                case (ResourceTypes.Other):
                default:
                    {
                        containerName = "other";
                        break;
                    }
            }


            // Create the container and return a container client object
            var response = blobServiceClient.GetBlobContainerClient(containerName)
                .CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            string path_project_bin = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));


            using (var fileStream = System.IO.File.OpenWrite(path_project_bin + "/bin/file-test/test" + blobName))
            {
                blobClient.DownloadTo(fileStream);

            }

            (this as IUploadMusicService).AES_Decrypt(path_project_bin + "/bin/file-test/test" + blobName, path_project_bin + "/bin/file-test/test-decrypt" + blobName, old_password);
            File.Delete(path_project_bin + "/bin/file-test/test" + blobName);

            (this as IUploadMusicService).AES_Encrypt(path_project_bin + "/bin/file-test/test-decrypt" + blobName, path_project_bin + "/bin/file-test/test-encrypt" + blobName, new_password);
            File.Delete(path_project_bin + "/bin/file-test/test-decrypt" + blobName);

            using (var EncryptfileStream = System.IO.File.OpenRead(path_project_bin + "/bin/file-test/test-encrypt" + blobName))
            {
                blobClient.DeleteIfExistsAsync();

                var fileName = System.IO.Path.GetFileNameWithoutExtension(path_project_bin + "/bin/file-test/test-encrypt" + blobName);
                var fileExtension = System.IO.Path.GetExtension(path_project_bin + "/bin/file-test/test-encrypt" + blobName);
                var uniqueFileName = fileName + "-" + Guid.NewGuid().ToString() + fileExtension;

                BlobClient blobClientEncrypt = containerClient.GetBlobClient(uniqueFileName);
                blobClientEncrypt.Upload(path_project_bin + "/bin/file-test/test-encrypt" + blobName);
            }

            string blobPath = "https://music98.blob.core.windows.net/video/";
            string pathName = blobPath + "test-encrypt" + blobName;

            File.Delete(path_project_bin + "/bin/file-test/test-encrypt" + blobName);
            blobClient.StartCopyFromUriAsync(blobClient.Uri);

            return pathName;
        }

        public string GetFileContentType(string FilePath)
        {
            string ContentType = string.Empty;
            string Extension = System.IO.Path.GetExtension(FilePath).ToLower();

            switch (Extension)
            {
                case "pdf":
                case ".pdf":
                    ContentType = "application/pdf";
                    break;
                case "bmp":
                case ".bmp":
                    ContentType = "image/bmp";
                    break;
                case "gif":
                case ".gif":
                    ContentType = "image/gif";
                    break;
                case "png":
                case ".png":
                    ContentType = "image/png";
                    break;
                case "jpg":
                case ".jpg":
                    ContentType = "image/jpeg";
                    break;
                case "jpeg":
                case ".jpeg":
                    ContentType = "image/jpeg";
                    break;
                case "zip":
                case ".zip":
                    ContentType = "application/zip";
                    break;
                default:
                    ContentType = "application/octet-stream";
                    break;

            }
            return ContentType;
        }

        string IUploadMusicService.GetFileUri(string fileName)
        {
            var resource = musicAssetRepository.Get(fileName);

            if (resource == null)
            {
                return null;
            }

            var resourceUri = resource.Uri;
            return resourceUri;
        }

        void IUploadMusicService.AES_Encrypt(string inputFile, string outputFile, string password)
        {
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged AES = new RijndaelManaged();

            AES.KeySize = 256;
            AES.BlockSize = 128;

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.Zeros;

            AES.Mode = CipherMode.CBC;

            CryptoStream cs = new CryptoStream(fsCrypt,
                 AES.CreateEncryptor(),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


            fsIn.Close();
            cs.Close();
            fsCrypt.Close();

        }

        void IUploadMusicService.AES_Decrypt(string inputFile, string outputFile, string password)
        {
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            RijndaelManaged AES = new RijndaelManaged();

            AES.KeySize = 256;
            AES.BlockSize = 128;

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.Zeros;

            AES.Mode = CipherMode.CBC;

            CryptoStream cs = new CryptoStream(fsCrypt,
                AES.CreateDecryptor(),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }
    }
}
