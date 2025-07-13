using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Models.Database
{
    public class MusicDBContext : DbContext
    { 
        public MusicDBContext(DbContextOptions<MusicDBContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<MusicInfo> MusicInfos { get; set; }
        public DbSet<MusicAsset> MusicAssets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MusicAssetTransfer> MusicAssetTransfers { get; set; }
        public DbSet<ShareOwnerShip> ShareOwnerShips { get; set; }
    }
}
