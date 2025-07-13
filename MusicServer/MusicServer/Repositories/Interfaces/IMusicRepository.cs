using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Interfaces
{
    public interface IMusicRepository
    {
        void Create(MusicInfo music);
        Guid CreateAndReturnId(MusicInfo music);

        void CreateMusicOwnerShip(ShareOwnerShip share);

        void Update(MusicInfo music);

        void UpdateKey(Guid musicId, string key1, string fullkey, uint ownerId, string musicLink);

        List<MusicInfo> GetAll();

        List<MusicQueryData> GetWithUserID(uint userID);

        List<MusicAssetTransfer> GetWithUserIDWithTFBuying(uint userID);

        MusicQueryData GetWithUserIDWithBuying(Guid musicId, string mediaLink, Guid transferId, bool isPermanent, bool isConfirmed);

        List<MusicQueryData> GetMusicShareOwnerShip(int userID);

        MusicInfo Get(Guid musicId);

        MusicInfo GetMusicWithTransactionHash(string transactionHash);
        MusicAssetTransfer GetMusicTFWithTransactionHash(string transactionHash);

        List<MusicInfo> GetOwnerId(Guid musicId);
        List<MusicAssetTransfer> GetBuyerId(Guid musicId);

        ShareOwnerShip GetMusicWithIdAndUserId(Guid musicId, int userId);

        void Delete(Guid id);
    }
}
