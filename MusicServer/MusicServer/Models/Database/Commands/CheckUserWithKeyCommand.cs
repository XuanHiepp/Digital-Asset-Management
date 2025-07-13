using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CheckUserWithKeyCommand
    {
        public Guid MusicId { get; set; }
        public string UserKeyInfo { get; set; }
        public BigInteger Key2X { get; set; }
    }
}
