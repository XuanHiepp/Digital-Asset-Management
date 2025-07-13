using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Services.Interfaces
{
    public interface IMusicService
    {
        /// <summary>
        /// Create a new album in the database and Ethereum network.
        /// </summary>
        /// <returns>Return the Id of the newly created album</returns>
        Task<Guid> Create(
            string name,
            string album,
            string publishingYear,
            uint ownerId,
            string licenceLink,
            string musicLink,
            string demoLink,
            string key1,
            string key2,
            string fullKey,
            CreatureTypes creatureType,
            OwnerTypes ownerType);

        void UpdateKey(
            Guid musicId,
            string key1,
            string fullKey,
            uint ownerId,
            string musicLink);

        Task UpdateOwnerForChangeOwnerShip(
            Guid id,
            uint ownerId,
            string musicLink);

        void CreateMusicOwnerShip(Guid musicId, int userId);

        List<MusicQueryData> GetAllMusics();
        MusicInfo GetMusicWithId(Guid musicId);
        List<MusicQueryData> GetMusicListWithUserID(uint userID);
        List<MusicQueryData> GetMusicListWithUserIDWithBuying(uint userID);
        List<int> GetOwnerBuyerId(Guid musicId);

        List<MusicQueryData> GetMusicShareOwnerShip(int userID);
        ShareOwnerShip GetMusicWithIdAndUserId(Guid musicId, int userId);

        Task<MusicInfoSC> GetMusicAsset(int ownerId, Guid id);
        Task<MusicInfoSC> GetMusicAssetForOrigin(string transactionHash);
        Task<MusicTransferSC> GetMusicTransferForOrigin(string transactionHash);

        MusicInfo GetMusicWithTransactionHash(string transactionHash);
        MusicAssetTransfer GetMusicTFWithTransactionHash(string transactionHash);
    }
}
