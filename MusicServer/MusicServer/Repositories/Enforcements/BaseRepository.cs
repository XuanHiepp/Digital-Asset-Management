using MusicServer.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Enforcements
{
    public class BaseRepository
    {
        public readonly MusicDBContext dbContext;
        public BaseRepository(MusicDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
