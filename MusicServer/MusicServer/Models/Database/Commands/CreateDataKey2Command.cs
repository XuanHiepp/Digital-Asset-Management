using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateDataKey2Command
    {
        public BigInteger pValue { get; set; }
        public BigInteger FullKey1X { get; set; }
        public int Key2X { get; set; }
        public int KeyType { get; set; }
    }
}
