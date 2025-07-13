using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicServer.Models.Database;
using MusicServer.Models.Database.Commands;
using MusicServer.Services.Interfaces;

namespace MusicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicAssetTransfersController : ControllerBase
    {
        private readonly IMusicAssetTransferService musicAssetTransferService;
        public MusicAssetTransfersController(IMusicAssetTransferService musicAssetTransferService)
        {
            this.musicAssetTransferService = musicAssetTransferService;
        }

        [HttpPost("CreateTransferAsync")]
        public async Task<IActionResult> CreateTransferAsync(
            [FromBody] CreateMusicAssetTransferCommand command
            )
        {
            try
            {
                if (command.FromUserId == command.ToUserId)
                {
                    return BadRequest("FromUserId cannot be equal to ToUserId.");
                }

                if (command.Duration <= 0)
                {
                    return BadRequest("Duration cannot be less than 0");
                }

                if (command.AmountValue <= 0)
                {
                    return BadRequest("AmountValue cannot be less than 0");
                }

                Enum.TryParse<TranTypes>(command.TranType, true, out TranTypes tranType);
                Enum.TryParse<FanTypes>(command.FanType, true, out FanTypes fanType);

                var result = await musicAssetTransferService.Create(
                    command.MusicId,
                    command.FromUserId,
                    command.ToUserId,
                    command.BuyerId,
                    tranType,
                    fanType,
                    command.Duration,
                    command.AmountValue,
                    command.Key2);
                return Ok(new { MusicAssetTransferId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("CreateLicenceTrans")]
        public async Task<IActionResult> CreateLicenceTransAsync(
            [FromBody] CreateMusicAssetTransferCommand command
            )
        {
            try
            {
                if (command.FromUserId == command.ToUserId)
                {
                    return BadRequest("FromUserId cannot be equal to ToUserId.");
                }

                if (command.AmountValue <= 0)
                {
                    return BadRequest("ValueAmount cannot be less than 0");
                }

                Enum.TryParse<TranTypes>(command.TranType, true, out TranTypes tranType);
                Enum.TryParse<FanTypes>(command.FanType, true, out FanTypes fanType);

                var result = await musicAssetTransferService.CreateLicenceTransaction(
                    command.MusicId,
                    command.FromUserId,
                    command.ToUserId,
                    command.BuyerId,
                    tranType,
                    fanType,
                    command.AmountValue);
                return Ok(new { MusicAssetTransferId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("UpdateLicenceTransAsync")]
        public async Task<IActionResult> UpdateLicenceTransAsync(
            [FromBody] CreateApproveTransferCommand command
            )
        {
            try
            {
                await musicAssetTransferService.UpdateLicenceTransaction(command.id, command.musicId);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("UpdateMediaLink")]
        public IActionResult UpdateMediaLink(
            [FromBody] CreateMediaLinkCommand command
            )
        {
            try
            {
                musicAssetTransferService.UpdateMediaLink(command.id, command.mediaLink);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("GetTransfers")]
        public IActionResult GetTransfers()
        {
            try
            {
                var result = musicAssetTransferService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("GetMusicTransfers/{id}")]
        public IActionResult GetMusicTransfers(Guid id)
        {
            try
            {
                var result = musicAssetTransferService.GetTransactMusic(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("GetLicenceMusicTransfers/{id}")]
        public IActionResult GetLicenceMusicTransfers(Guid id)
        {
            try
            {
                var result = musicAssetTransferService.GetLicenceTransactMusic(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("GetLicenceMusicTransfersSC/{id}")]
        public IActionResult GetLicenceMusicTransfersSC(Guid id)
        {
            try
            {
                var result = musicAssetTransferService.GetLicenceMusicTransfersSC(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        //[HttpGet]
        //[Route("{musicId}/contract-address")]
        //public async Task<IActionResult> GetContractAddress(Guid musicId)
        //{
        //    try
        //    {
        //        var result = await musicAssetTransferService.GetContractAddress(musicId);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex);
        //    }
        //}


        [HttpGet("GetTransfer/{id}")]
        public IActionResult GetTransfer(Guid id)
        {
            try
            {
                var result = musicAssetTransferService.GetSC(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("{musicTFId}/contract-address")]
        public async Task<IActionResult> GetInfoContractAddress(Guid musicTFId)
        {
            try
            {
                var result = await musicAssetTransferService.GetMusicTransfer(musicTFId);
                return Ok(new { Key2 = result.Key2 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}