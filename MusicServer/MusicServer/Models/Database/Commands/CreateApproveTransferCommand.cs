using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateApproveTransferCommand
    {
        public Guid id { get; set; }
        public Guid musicId { get; set; }
    }
}
