using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using MusicServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Enforcements
{
    public class MusicRepository : BaseRepository, IMusicRepository
    {
        public MusicRepository(MusicDBContext dbContext) : base(dbContext)
        {
        }

        void IMusicRepository.Create(MusicInfo music)
        {
            dbContext.MusicInfos.Add(music);
            dbContext.SaveChanges(true);
        }

        Guid IMusicRepository.CreateAndReturnId(MusicInfo music)
        {
            (this as IMusicRepository).Create(music);
            return music.Id;
        }

        void IMusicRepository.CreateMusicOwnerShip(ShareOwnerShip share)
        {
            dbContext.ShareOwnerShips.Add(share);
            dbContext.SaveChanges(true);
        }

        void IMusicRepository.Delete(Guid id)
        {
            var musicToBeDeleted = (this as IMusicRepository).Get(id);
            dbContext.MusicInfos.Remove(musicToBeDeleted);
            dbContext.SaveChanges();
        }

        MusicInfo IMusicRepository.Get(Guid musicId)
        {
            var result = dbContext.MusicInfos.Where(c => c.Id == musicId).SingleOrDefault();
            return result;
        }

        MusicInfo IMusicRepository.GetMusicWithTransactionHash(string transactionHash)
        {
            var result = from a in dbContext.MusicAssetTransfers
                         join b in dbContext.MusicInfos
                         on a.MusicId equals b.Id
                         where a.TransactionHash == transactionHash
                         select b;
            return result.FirstOrDefault();
        }

        MusicAssetTransfer IMusicRepository.GetMusicTFWithTransactionHash(string transactionHash)
        {
            var result = from a in dbContext.MusicAssetTransfers
                         where a.TransactionHash == transactionHash
                         select a;
            return result.FirstOrDefault();
        }

        List<MusicInfo> IMusicRepository.GetOwnerId(Guid musicId)
        {
            var result = dbContext.MusicInfos.Where(c => c.Id == musicId);
            return result.ToList();
        }

        ShareOwnerShip IMusicRepository.GetMusicWithIdAndUserId(Guid musicId, int userId)
        {
            var share = dbContext.ShareOwnerShips.Where(c => (c.MusicId == musicId && c.UserId == userId));
            return share.FirstOrDefault();
        }

        List<MusicAssetTransfer> IMusicRepository.GetBuyerId(Guid musicId)
        {
            var result = from a in dbContext.MusicInfos
                         join b in dbContext.MusicAssetTransfers
                         on a.Id equals b.MusicId
                         where a.Id == musicId
                         select b;
            return result.ToList();
        }

        List<MusicInfo> IMusicRepository.GetAll()
        {
            return dbContext.MusicInfos.ToList();
        }

        List<MusicQueryData> IMusicRepository.GetWithUserID(uint userID)
        {
            var result = from a in dbContext.MusicInfos
                         where a.OwnerId == userID
                         select new MusicQueryData()
                         {
                            Id = a.Id,
                            Name = a.Name,
                            Title = a.Title,
                            Album = a.Album,
                            PublishingYear = a.PublishingYear,
                            OwnerId = a.OwnerId,
                            LicenceLink = a.LicenceLink,
                            MusicLink = a.MusicLink,
                            CreatureType = a.CreatureType.ToString(),
                            OwnerType = a.OwnerType.ToString(),
                            TransactionHash = a.TransactionHash,
                            ContractAddress = a.ContractAddress,
                            DateCreated = a.DateCreated,
                            TransactionStatus = a.TransactionStatus.ToString(),
                            FullKey = a.FullKey
                         };
            return result.ToList();
        }

        List<MusicAssetTransfer> IMusicRepository.GetWithUserIDWithTFBuying(uint userID)
        {
            var result = from a in dbContext.MusicAssetTransfers
                         where a.BuyerId == userID
                         select a;
            return result.ToList();
        }

        MusicQueryData IMusicRepository.GetWithUserIDWithBuying(Guid musicId, string mediaLink, Guid transferId, bool isPermanent, bool isConfirmed)
        {
            var result = from a in dbContext.MusicInfos
                         where a.Id == musicId
                         select new MusicQueryData()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Title = a.Title,
                             Album = a.Album,
                             PublishingYear = a.PublishingYear,
                             OwnerId = a.OwnerId,
                             LicenceLink = a.LicenceLink,
                             MusicLink = a.MusicLink,
                             Key1 = a.Key1,
                             FullKey = a.FullKey,
                             MediaLink = mediaLink,
                             CreatureType = a.CreatureType.ToString(),
                             OwnerType = a.OwnerType.ToString(),
                             TransactionHash = a.TransactionHash,
                             ContractAddress = a.ContractAddress,
                             DateCreated = a.DateCreated,
                             TransactionStatus = a.TransactionStatus.ToString(),
                             TransferId = transferId,
                             IsPermanent = isPermanent,
                             IsConfirmed = isConfirmed
                         };
            return result.First();
        }

        List<MusicQueryData> IMusicRepository.GetMusicShareOwnerShip(int userID)
        {
            var result = from a in dbContext.ShareOwnerShips
                         join b in dbContext.MusicInfos
                         on a.MusicId equals b.Id
                         where a.UserId == userID
                         select new MusicQueryData()
                         {
                             Id = b.Id,
                             Name = b.Name,
                             Title = b.Title,
                             Album = b.Album,
                             PublishingYear = b.PublishingYear,
                             OwnerId = b.OwnerId,
                             LicenceLink = b.LicenceLink,
                             MusicLink = b.MusicLink,
                             CreatureType = b.CreatureType.ToString(),
                             OwnerType = b.OwnerType.ToString(),
                             TransactionHash = b.TransactionHash,
                             ContractAddress = b.ContractAddress,
                             DateCreated = b.DateCreated,
                             TransactionStatus = b.TransactionStatus.ToString(),
                             FullKey = b.FullKey,
                             Key1 = b.Key1
                         };
            return result.ToList();
        }

        void IMusicRepository.Update(MusicInfo music)
        {
            dbContext.MusicInfos.Update(music);
            dbContext.SaveChanges();
        }

        void IMusicRepository.UpdateKey(Guid musicId, string key1, string fullkey, uint ownerId, string musicLink)
        {
            var model = dbContext.MusicInfos.FirstOrDefault(x => x.Id == musicId);
            model.Key1 = key1;
            model.FullKey = fullkey;
            model.OwnerId = ownerId;
            model.MusicLink = musicLink;
            dbContext.SaveChanges();
        }
    }
}
