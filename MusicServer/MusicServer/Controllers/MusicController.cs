using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicServer.Models.Database;
using MusicServer.Models.Database.Commands;
using MusicServer.Models.Database.Queries;
using MusicServer.Services.Interfaces;
using Org.BouncyCastle.Crypto;

namespace MusicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService musicService;
        private readonly IPKIEncordeService pkiEncordeService;

        public MusicController(IMusicService musicService,
            IPKIEncordeService pkiEncordeService)
        {
            this.musicService = musicService;
            this.pkiEncordeService = pkiEncordeService;
        }

        [HttpGet]
        public List<MusicQueryData> GetMusics()
        {
            var musics = musicService.GetAllMusics();
            return musics;
        }

        // GET: api/Music/5
        [HttpGet("GetMusicWithId/{musicId}")]
        public MusicInfo GetMusicWithId(Guid musicId)
        {
            try
            {
                var music = musicService.GetMusicWithId(musicId);
                return music;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetMusicShareOwnerShip/{userId}")]
        public List<MusicQueryData> GetMusicShareOwnerShip(int userId)
        {
            var musics = musicService.GetMusicShareOwnerShip(userId);
            return musics;
        }

        [HttpGet("GetMusicWithTransactionHash/{transactionHash}")]
        public MusicInfo GetMusicWithTransactionHash(string transactionHash)
        {
            var music = musicService.GetMusicWithTransactionHash(transactionHash);
            return music;
        }

        [HttpGet("GetMusicTFWithTransactionHash/{transactionHash}")]
        public MusicAssetTransfer GetMusicTFWithTransactionHash(string transactionHash)
        {
            var music = musicService.GetMusicTFWithTransactionHash(transactionHash);
            return music;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMusicAsync([FromBody] CreateMusicCommand command)
        {
            try
            {
                if (string.IsNullOrEmpty(command.Name))
                {
                    command.Name = string.Empty;
                }

                if (command.Title == null)
                {
                    command.Title = string.Empty;
                }
                if (command.Album == null)
                {
                    command.Album = string.Empty;
                }
                if (command.PublishingYear == null)
                {
                    command.PublishingYear = string.Empty;
                }

                Enum.TryParse<CreatureTypes>(command.CreatureType, true, out CreatureTypes creatureType);
                Enum.TryParse<OwnerTypes>(command.OwnerType, true, out OwnerTypes ownerTypes);

                var result = await musicService.Create(
                    command.Name,
                    command.Album,
                    command.PublishingYear,
                    command.OwnerId,
                    command.LicenceLink,
                    command.MusicLink,
                    command.DemoLink,
                    command.Key1,
                    command.Key2,
                    command.FullKey,
                    creatureType,
                    ownerTypes);

                return Ok(new { MusicId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("UpdateChangeOwnerShipAsync")]
        public async Task<IActionResult> UpdateChangeOwnerShipAsync(
            [FromBody] CreateChangeOwnerShipCommand command
            )
        {
            try
            {
                await musicService.UpdateOwnerForChangeOwnerShip(command.id, command.ownerId, command.musicLink);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("UpdateKey")]
        public IActionResult UpdateKey([FromBody] CreateMusicKeyCommand command)
        {
            musicService.UpdateKey(command.MusicId, command.Key1, command.FullKey, command.OwnerId, command.MusicLink);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("CreateMusicOwnerShip")]
        public IActionResult CreateMusicOwnerShip([FromBody] CreateMusicOwnerShipCommand command)
        {
            var musicCheck = musicService.GetMusicWithIdAndUserId(command.MusicId, command.UserId);
            if (musicCheck != null)
            {
                return Ok(new { CheckExist = true });
            }
            else
            {
                musicService.CreateMusicOwnerShip(command.MusicId, command.UserId);
                return Ok(new { CheckExist = false });
            }         
        }

        [HttpPost("SignDataKey1")]
        public IActionResult SignDataKey1([FromBody] CreateDataKey1Command command)
        {
            try
            {
                SHA256Managed sha256 = new SHA256Managed();
                BigInteger p = pkiEncordeService.RandomInteger(10); //generate a random Big Integer number
                BigInteger key1 = BigInteger.Pow(p, command.Key1X);
                string data = key1.ToString();
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashedMessage = sha256.ComputeHash(dataBytes);

                command.KeyType = 1;

                var keyPair = pkiEncordeService.GetKeyPair(command.KeyType, command.UserID);

                var signature = pkiEncordeService.SignData(hashedMessage, keyPair.Private);

                return Ok(new { hashMess = hashedMessage, Sign = signature, pValue = p, Key1 = key1 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("SignDataShareFullKey")]
        public IActionResult SignDataShareFullKey([FromBody] CreateDataShareFullKeyCommand command)
        {
            try
            {
                SHA256Managed sha256 = new SHA256Managed();
                string data = command.FullKey.ToString();
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashedMessage = sha256.ComputeHash(dataBytes);

                command.KeyType = 1;

                var keyPair = pkiEncordeService.GetKeyPair(command.KeyType, command.UserID);

                var signature = pkiEncordeService.SignData(hashedMessage, keyPair.Private);

                return Ok(new { hashMess = hashedMessage, Sign = signature, FullKey = data });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("SignDataKey2Server")]
        public IActionResult SignDataKey2Server([FromBody] CreateDataKey2Command command)
        {
            try
            {
                SHA256Managed sha256 = new SHA256Managed();
                BigInteger pValue = command.pValue;
                BigInteger key2 = BigInteger.Pow(pValue, command.Key2X);
                BigInteger fullKey = command.FullKey1X * key2;
                string data = key2.ToString();
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashedMessage = sha256.ComputeHash(dataBytes);

                command.KeyType = 0;

                var keyPair = pkiEncordeService.GetKeyPair(command.KeyType, 0);

                var signature = pkiEncordeService.SignData(hashedMessage, keyPair.Private);

                return Ok(new { hashMess = hashedMessage, Sign = signature, FullKey = fullKey, Key2 = key2 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("CreateFullKey")]
        public IActionResult CreateFullKey([FromBody] CreateFullKeyCommand command)
        {
            try
            {
                BigInteger fullKey = command.Key1X * command.Key2X;

                return Ok(new { FullKey = fullKey });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("CheckUserWithKeyExist/{infoCheckExist}")]
        public IActionResult CheckUserWithKeyExist(string infoCheckExist)
        {
            try
            {
                Guid musicId = Guid.Parse(infoCheckExist.Split("_")[0]);
                int userId = int.Parse(infoCheckExist.Split("_")[1]);
                List<int> userExistList = musicService.GetOwnerBuyerId(musicId);

                if (userExistList.Contains(userId))
                {
                    return Ok(new { existCheck = true });
                }
                else
                {
                    return Ok(new { existCheck = false });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("VerifySignature")]
        public  IActionResult VerifySignature([FromBody] CreateDataCheckSignCommand command)
        {
            //try
            //{
            AsymmetricCipherKeyPair keyPair = null;
            if (command.KeyType == 0)
            {

                keyPair = pkiEncordeService.GetKeyPair(command.KeyType, 0);
            }
            else
            {
                keyPair = pkiEncordeService.GetKeyPair(command.KeyType, command.UserID);
            }
                

            var valid = pkiEncordeService.VerifySignature(command.hashedMessage, command.signature, keyPair.Public);
                
            if (valid == true)
            {
                return Ok(new { checkSign = valid });
            }
            else
            {
                return Ok(new { checkSign = valid });
            }

                
            //}
            //catch (Exception ex)
            //{
            //    //return StatusCode(StatusCodes.Status500InternalServerError, ex);
            //    return Ok(new { checkSign = false });
            //}
        }

        [HttpGet]
        [Route("{ownerInfo}/contract-address")]
        public async Task<IActionResult> GetInfoContractAddress(string ownerInfo)
        {
            try
            {
                Guid musicId = Guid.Parse(ownerInfo.Split("_")[0]);
                int ownerId = int.Parse(ownerInfo.Split("_")[1]);
                var result = await musicService.GetMusicAsset(ownerId, musicId);
                return Ok(new { Key2 = result.Key2 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("GetMusicForOrigin/{transactionHash}")]
        public async Task<IActionResult> GetMusicForOrigin(string transactionHash)
        {
            try
            {
                var result = await musicService.GetMusicAssetForOrigin(transactionHash);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("GetMusicTransferForOrigin/{transactionHash}")]
        public async Task<IActionResult> GetMusicTransferForOrigin(string transactionHash)
        {
            try
            {
                var result = await musicService.GetMusicTransferForOrigin(transactionHash);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
