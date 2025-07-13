using MusicServer.Models.Database;
using MusicServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Enforcements
{
    public class MusicAssetRepository : BaseRepository, IMusicAssetRepository
    {
        public MusicAssetRepository(MusicDBContext dbContext) : base(dbContext)
        {
        }

        void IMusicAssetRepository.Create(MusicAsset file)
        {
            dbContext.MusicAssets.Add(file);
            dbContext.SaveChanges();
        }

        string IMusicAssetRepository.CreateAndReturnKey(MusicAsset file)
        {
            (this as IMusicAssetRepository).Create(file);
            return file.Name;
        }

        void IMusicAssetRepository.Delete(string name)
        {
            var recordToBeDeleted = (this as IMusicAssetRepository).Get(name);
            dbContext.MusicAssets.Remove(recordToBeDeleted);
            dbContext.SaveChanges();
        }

        MusicAsset IMusicAssetRepository.Get(string name)
        {
            var result = dbContext.MusicAssets.Where(r => r.Name == name).SingleOrDefault();
            return result;
        }

        List<MusicAsset> IMusicAssetRepository.GetAll()
        {
            var result = dbContext.MusicAssets.ToList();
            return result;
        }

        void IMusicAssetRepository.Update(MusicAsset file)
        {
            dbContext.MusicAssets.Update(file);
            dbContext.SaveChanges();
        }
    }
}
