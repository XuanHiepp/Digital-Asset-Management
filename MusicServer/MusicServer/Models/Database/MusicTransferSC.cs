using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    [FunctionOutput]
    public class MusicTransferSC : IFunctionOutputDTO
    {
        [Parameter("string", 1)]
        public string Id { get; set; }
        [Parameter("string", 2)]
        public string MusicId { get; set; }
        [Parameter("string", 3)]
        public string FromAddress { get; set; }
        [Parameter("string", 4)]
        public string ToAddress { get; set; }
        [Parameter("string", 5)]
        public string Key2 { get; set; }
    }
}
