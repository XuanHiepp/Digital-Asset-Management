using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateChangeOwnerShipCommand
    {
        public Guid id { get; set; }
        public uint ownerId { get; set; }
        public string musicLink { get; set; }
    }
}
