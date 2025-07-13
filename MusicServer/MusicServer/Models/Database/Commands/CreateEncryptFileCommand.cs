using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database.Commands
{
    public class CreateEncryptFileCommand
    {
        public IFormFile myFile { get; set; }
        public string password { get; set; }
    }
}
