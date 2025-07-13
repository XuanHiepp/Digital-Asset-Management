using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class DownloadEncryptFileCommand
    {
        public string blobName { get; set; }
        public string password { get; set; }
    }
}
