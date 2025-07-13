using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    [Function("getMusicAssetTransfer")]
    public class MusicAssetTFInput : FunctionMessage
    {
        [Parameter("string", "_guid", 1)]
        public string Id { get; set; }
    }
}
