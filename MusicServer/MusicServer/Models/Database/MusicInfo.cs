using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    public class MusicInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string PublishingYear { get; set; }
        public uint OwnerId { get; set; }
        public string LicenceLink { get; set; }
        public string MusicLink { get; set; }
        public string DemoLink { get; set; }

        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string FullKey { get; set; }

        public uint LicencePrice { get; set; }

        public CreatureTypes CreatureType { get; set; }
        public OwnerTypes OwnerType { get; set; }

        public string TransactionHash { get; set; }
        public string ContractAddress { get; set; }
        public DateTime? DateCreated { get; set; }

        public TransactionStatuses TransactionStatus { get; set; }
    }
}
