using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateMusicOwnerShipCommand
    {
        public Guid MusicId { get; set; }
        public int UserId { get; set; }
    }
}
