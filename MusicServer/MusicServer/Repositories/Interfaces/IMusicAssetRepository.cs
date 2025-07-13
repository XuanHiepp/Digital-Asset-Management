using MusicServer.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Interfaces
{
    public interface IMusicAssetRepository
    {
        void Create(MusicAsset file);
        string CreateAndReturnKey(MusicAsset file);

        void Update(MusicAsset file);

        List<MusicAsset> GetAll();
        MusicAsset Get(string id);

        void Delete(string id);
    }
}
