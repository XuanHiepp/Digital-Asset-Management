using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class DownloadEncryptFileOwnerShipCommand
    {
        public string blobName { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
    }
}
