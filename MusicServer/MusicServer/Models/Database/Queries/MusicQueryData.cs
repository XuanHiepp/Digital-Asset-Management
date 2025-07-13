using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Queries
{
    public class MusicQueryData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string PublishingYear { get; set; }
        public uint OwnerId { get; set; }
        public string LicenceLink { get; set; }
        public string MusicLink { get; set; }
        public string Key1 { get; set; }
        public string FullKey { get; set; }
        public string CreatureType { get; set; }
        public string OwnerType { get; set; }

        public bool IsPermanent { get; set; }
        public bool IsConfirmed { get; set; }

        public Guid TransferId { get; set; }

        public string MediaLink { get; set; }

        public string TransactionHash { get; set; }
        public string ContractAddress { get; set; }
        public DateTime? DateCreated { get; set; }
        public string TransactionStatus { get; set; }
    }
}
