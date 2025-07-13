using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    [FunctionOutput]
    public class MusicAssetTF
    {
        [Parameter("string", "id", 1)]
        public string Id { get; set; }

        [Parameter("string", "musicAssetId", 2)]
        public string MusicAssetId { get; set; }

        [Parameter("string", "fromOwnerId", 3)]
        public string FromOwnerId { get; set; }

        [Parameter("string", "toFanId", 4)]
        public string ToFanId { get; set; }

        [Parameter("uint256", "quantity", 5)]
        public BigInteger Quantity { get; set; }

        [Parameter("uint256", "dateTransferred", 6)]
        public BigInteger dateTransferred { get; set; }
    }
}
