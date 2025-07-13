using Microsoft.Extensions.Options;
using MusicServer.Models.Database;
using MusicServer.Repositories.Interfaces;
using MusicServer.Services;
using MusicServer.Services.Interfaces;
using MusicServer.UndergroundJobs.Interfaces;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.UndergroundJobs.Enforcements
{
    public class MusicUndergroundJob : IMusicUndergroundJob
    {
        private readonly IEthereumService ethereumService;
        private readonly IMusicRepository musicRepository;
        private readonly IMusicService musicService;
        private readonly string MusicAbi;

        public MusicUndergroundJob(
            IEthereumService ethereumService,
            IMusicRepository musicRepository,
            IMusicService musicService,
            IOptions<EthereumSettings> options)
        {
            this.ethereumService = ethereumService;
            this.musicRepository = musicRepository;
            this.musicService = musicService;
            MusicAbi = options.Value.MusicAbi;
        }

        void IMusicUndergroundJob.SyncDatabaseWithBlockchain()
        {

        }

        void IMusicUndergroundJob.WaitForTransactionToSuccessThenFinishCreatingMusic(MusicInfo music)
        {
            bool isTransactionSuccess = false;
            do
            {
                var receipt = ethereumService.GetTransactionReceipt(music.TransactionHash).Result;
                if (receipt == null)
                    continue;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    isTransactionSuccess = true;
                    var contractAddress = ethereumService.GetObjectContractAddress(music.Id).Result;
                    music.ContractAddress = contractAddress;
                    music.TransactionStatus = TransactionStatuses.Success;
                    musicRepository.Update(music);

                    var musicContract = ethereumService.GetContract(MusicAbi, music.ContractAddress);
                    var updateFunction = ethereumService.GetFunction(musicContract, EthereumFunctions.UpdateMusicInformation);
                    var updateReceipt = updateFunction.SendTransactionAndWaitForReceiptAsync(
                        ethereumService.GetEthereumAccount(),
                        new HexBigInteger(6000000),
                        new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                        new HexBigInteger(0),
                        functionInput: new object[] {
                            music.Name,
                            music.Title,
                            music.Album,
                            music.PublishingYear,
                            music.OwnerId,
                            music.LicenceLink,
                            (uint)music.CreatureType,
                            (uint)music.OwnerType
                        }).Result;
                }

            }
            while (isTransactionSuccess != true);
        }

    }
}
