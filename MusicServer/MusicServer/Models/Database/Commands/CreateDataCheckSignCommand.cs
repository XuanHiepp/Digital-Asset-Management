using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateDataCheckSignCommand
    {
        public byte[] hashedMessage { get; set; }
        public byte[] signature { get; set; }
        public int KeyType { get; set; }
        public int UserID { get; set; }
    }
}
