using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Utilities
{
    public static class MusicAssetTransferExtensionMethods
    {
        public static MusicAssetTransferQueryData ToMusicAssetTransferQueryData(this MusicAssetTransfer transfer)
        {
            if (transfer == null) return null;
            var result = new MusicAssetTransferQueryData()
            {
                From = transfer.From,
                To = transfer.To,
                Id = transfer.Id,
                MusicInfo = transfer.MusicInfo,
                IsConfirmed = transfer.IsConfirmed,
                ContractAddress = transfer.ContractAddress,
                DateCreated = transfer.DateCreated,
                TransactionHash = transfer.TransactionHash,
                TransactionStatus = transfer.TransactionStatus.ToString()
            };
            return result;
        }
    }
}
