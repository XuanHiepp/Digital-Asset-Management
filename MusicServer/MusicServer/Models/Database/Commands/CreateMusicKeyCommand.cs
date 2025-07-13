using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateMusicKeyCommand
    {
        public Guid MusicId { get; set; }
        public string Key1 { get; set; }
        public string FullKey { get; set; }
        public uint OwnerId { get; set; }
        public string MusicLink { get; set; }
    }
}
