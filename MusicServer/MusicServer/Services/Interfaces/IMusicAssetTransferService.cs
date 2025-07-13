using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Services.Interfaces
{
    public interface IMusicAssetTransferService
    {
        Task<Guid> Create(
            Guid musicId,
            string fromUserId,
            string toUserId,
            int buyerId,
            TranTypes tranType,
            FanTypes fanType,
            int duration,
            decimal amountValue,
            string key2
            );

        Task<Guid> CreateLicenceTransaction(
            Guid musicId,
            string fromUserId,
            string toUserId,
            int buyerId,
            TranTypes tranType,
            FanTypes fanType,
            decimal amountValue
            );


        Task UpdateLicenceTransaction(Guid id, Guid musicId);

        void UpdateMediaLink(Guid id, string mediaLink);


        List<MusicAssetTransferQueryData> GetAll();
        List<MusicAssetTransfer> GetTransactMusic(Guid id);
        List<MusicAssetTransfer> GetLicenceTransactMusic(Guid id);

        MusicAssetTF GetLicenceMusicTransfersSC(Guid id);

        MusicAssetTransfer Get(Guid id);

        MusicAssetTransfer GetSC(Guid id);

        /// <summary>
        /// Get contract address of a tenant on blockchain network.
        /// </summary>
        /// <param name="id"></param>
        //Task<string> GetContractAddress(Guid id);

        Task Delete(Guid id);

        Task<MusicTransferSC> GetMusicTransfer(Guid id);
    }
}
