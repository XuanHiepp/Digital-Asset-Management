using MusicServer.Models.Database;
using MusicServer.Repositories.Interfaces;
using MusicServer.Services.Interfaces;
using MusicServer.UndergroundJobs.Interfaces;
using Nethereum.Hex.HexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.UndergroundJobs.Enforcements
{
    public class MusicAssetTransferUndergroundJob : IMusicAssetTransferUndergroundJob
    {
        private readonly IEthereumService ethereumService;
        private readonly IMusicAssetTransferRepository musicAssetTransferRepository;
        public MusicAssetTransferUndergroundJob(
            IEthereumService ethereumService,
            IMusicAssetTransferRepository musicAssetTransferRepository)
        {
            this.ethereumService = ethereumService;
            this.musicAssetTransferRepository = musicAssetTransferRepository;
        }

        void IMusicAssetTransferUndergroundJob.WaitForTransactionToSuccessThenFinishCreating(MusicAssetTransfer transfer)
        {
            bool isTransactionSuccess = false;
            do
            {
                var receipt = ethereumService.GetTransactionReceipt(transfer.TransactionHash).Result;
                if (receipt == null)
                    continue;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    isTransactionSuccess = true;
                    var contractAddress = ethereumService.GetObjectContractAddress(transfer.Id).Result;
                    transfer.ContractAddress = contractAddress;
                    transfer.TransactionStatus = TransactionStatuses.Success;
                    musicAssetTransferRepository.Update(transfer);

                }
            }
            while (isTransactionSuccess != true);
        }
    }
}
