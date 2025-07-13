using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    public class CreateMediaLinkCommand
    {
        public Guid id { get; set; }
        public string mediaLink { get; set; }
    }
}
