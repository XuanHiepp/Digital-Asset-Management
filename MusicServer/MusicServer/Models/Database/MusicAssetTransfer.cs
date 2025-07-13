using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    public class MusicAssetTransfer : BlockchainObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid MusicId { get; set; }
        public MusicInfo MusicInfo { get; set; }

        [Required]
        public string FromId { get; set; }
        public User From { get; set; }

        [Required]
        public string ToId { get; set; }
        public User To { get; set; }

        public int BuyerId { get; set; }
        public string Key2 { get; set; }
        public string MediaLink { get; set; }

        public uint DateTransferred { get; set; }

        public TranTypes TranType { get; set; }
        public FanTypes FanType { get; set; }

        public uint DateStart { get; set; }
        public uint DateEnd { get; set; }

        public bool IsPermanent { get; set; }

        /// <summary>
        /// The sender creates the transfer, but it's not guaranteed on the other end. Did the recipient of the transfer confirm? 
        /// </summary>
        public bool IsConfirmed { get; set; } = false;

        public decimal AmountValue { get; set; }
    }
}
