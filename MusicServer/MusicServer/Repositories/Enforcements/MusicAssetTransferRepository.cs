using Microsoft.EntityFrameworkCore;
using MusicServer.Models.Database;
using MusicServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Enforcements
{
    public class MusicAssetTransferRepository : BaseRepository, IMusicAssetTransferRepository
    {
        public MusicAssetTransferRepository(MusicDBContext dbContext) : base(dbContext)
        {
        }

        void IMusicAssetTransferRepository.Create(MusicAssetTransfer musicAssetTransfer)
        {
            dbContext.MusicAssetTransfers.Add(musicAssetTransfer);
            dbContext.SaveChanges();
        }

        Guid IMusicAssetTransferRepository.CreateAndReturnId(MusicAssetTransfer musicAssetTransfer)
        {
            (this as IMusicAssetTransferRepository).Create(musicAssetTransfer);
            return musicAssetTransfer.Id;
        }

        void IMusicAssetTransferRepository.Delete(Guid id)
        {
            var recordToBeDeleted = (this as IMusicAssetTransferRepository).Get(id);
            dbContext.MusicAssetTransfers.Remove(recordToBeDeleted);
            dbContext.SaveChanges();
        }

        MusicAssetTransfer IMusicAssetTransferRepository.Get(Guid id)
        {
            var result = from a in dbContext.MusicAssetTransfers
                         where a.Id == id
                        select new MusicAssetTransfer()
                        {
                            Id = a.Id,
                            MusicId = a.MusicId,
                            FromId = a.FromId,
                            ToId = a.ToId,
                            TranType = a.TranType,
                            FanType = a.FanType,
                            DateStart = a.DateStart,
                            DateEnd = a.DateEnd,
                            TransactionHash = a.TransactionHash,
                            ContractAddress = a.ContractAddress,
                            DateCreated = a.DateCreated,
                            TransactionStatus = a.TransactionStatus,
                            AmountValue = a.AmountValue
                        };
            return result.First();
        }

        List<MusicAssetTransfer> IMusicAssetTransferRepository.GetAll()
        {
            return dbContext.MusicAssetTransfers
                .Include(t => t.MusicInfo)
                .Include(t => t.From)
                .Include(t => t.To)
                .ToList();
        }

        List<MusicAssetTransfer> IMusicAssetTransferRepository.GetTransactMusic(Guid id)
        {
            var result = from a in dbContext.MusicAssetTransfers
                         where a.IsPermanent == false
                         where a.MusicId == id
                         select new MusicAssetTransfer()
                         {
                             Id = a.Id,
                             MusicId = a.MusicId,
                             FromId = a.FromId,
                             ToId = a.ToId,
                             BuyerId = a.BuyerId,
                             TranType = a.TranType,
                             FanType = a.FanType,
                             DateStart = a.DateStart,
                             DateEnd = a.DateEnd,
                             TransactionHash = a.TransactionHash,
                             ContractAddress = a.ContractAddress,
                             DateCreated = a.DateCreated,
                             TransactionStatus = a.TransactionStatus,
                             AmountValue = a.AmountValue
                         };
            return result.ToList();
        }

        List<MusicAssetTransfer> IMusicAssetTransferRepository.GetLicenceTransactMusic(Guid id)
        {
            var result = from a in dbContext.MusicAssetTransfers
                         where a.IsPermanent == true
                         where a.MusicId == id
                         select new MusicAssetTransfer()
                         {
                             Id = a.Id,
                             MusicId = a.MusicId,
                             FromId = a.FromId,
                             ToId = a.ToId,
                             BuyerId = a.BuyerId,
                             TranType = a.TranType,
                             FanType = a.FanType,
                             DateStart = a.DateStart,
                             DateEnd = a.DateEnd,
                             TransactionHash = a.TransactionHash,
                             ContractAddress = a.ContractAddress,
                             DateCreated = a.DateCreated,
                             TransactionStatus = a.TransactionStatus,
                             AmountValue = a.AmountValue,
                             IsConfirmed = a.IsConfirmed
                         };
            return result.ToList();
        }

        void IMusicAssetTransferRepository.Update(MusicAssetTransfer musicAssetTransfer)
        {
            dbContext.MusicAssetTransfers.Update(musicAssetTransfer);
            dbContext.SaveChanges();
        }

        User IMusicAssetTransferRepository.GetUserInfo(int userID)
        {
            var result = dbContext.Users.Where(c => c.UserID == userID).SingleOrDefault();
            return result;
        }

        MusicAssetTransfer IMusicAssetTransferRepository.GetSC(Guid musicId)
        {
            var result = dbContext.MusicAssetTransfers.Where(c => c.Id == musicId).SingleOrDefault();
            return result;
        }
    }
}
