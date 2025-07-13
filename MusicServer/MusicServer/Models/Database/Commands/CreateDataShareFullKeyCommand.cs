using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateDataShareFullKeyCommand
    {
        public BigInteger FullKey { get; set; }
        public int KeyType { get; set; }
        public int UserID { get; set; }
    }
}
