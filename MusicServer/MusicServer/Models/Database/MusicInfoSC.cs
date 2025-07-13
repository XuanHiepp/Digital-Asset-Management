using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    [FunctionOutput]
    public class MusicInfoSC: IFunctionOutputDTO
    {
        [Parameter("string", 1)]
        public string Name { get; set; }
        [Parameter("string", 2)]
        public string Album { get; set; }
        [Parameter("string", 3)]
        public string PublishingYear { get; set; }
        [Parameter("uint256", 4)]
        public uint OwnerId { get; set; }
        [Parameter("string", 5)]
        public string Key2 { get; set; }
    }
}
