using MusicServer.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Interfaces
{
    public interface IMusicAssetTransferRepository
    {
        void Create(MusicAssetTransfer musicAssetTransfer);
        Guid CreateAndReturnId(MusicAssetTransfer musicAssetTransfer);

        void Update(MusicAssetTransfer musicAssetTransfer);

        List<MusicAssetTransfer> GetAll();
        List<MusicAssetTransfer> GetTransactMusic(Guid id);
        List<MusicAssetTransfer> GetLicenceTransactMusic(Guid id);
        MusicAssetTransfer Get(Guid id);

        void Delete(Guid id);

        User GetUserInfo(int userID);

        MusicAssetTransfer GetSC(Guid id);
    }
}
