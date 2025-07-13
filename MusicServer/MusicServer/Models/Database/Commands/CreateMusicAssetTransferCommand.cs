using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateMusicAssetTransferCommand
    {
        public Guid MusicId { get; set; }

        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

        public int BuyerId { get; set; }

        public string Key2 { get; set; }

        public string TranType { get; set; }
        public string FanType { get; set; }

        public int Duration { get; set; }
        public decimal AmountValue { get; set; }

        public bool IsPermanent { get; set; }
        
    }
}
