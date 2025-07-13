using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Queries
{
    public class MusicAssetTransferQueryData
    {
        public Guid Id { get; set; }

        public MusicInfo MusicInfo { get; set; }

        public User From { get; set; }
        public User To { get; set; }

        public bool IsConfirmed { get; set; }

        public string TransactionHash { get; set; }
        public string ContractAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public string TransactionStatus { get; set; }
    }
}
