using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Utilities
{
    public static class MusicExtensionMethods
    {
        public static MusicQueryData ToMusicQueryData(this MusicInfo music)
        {
            if (music == null) return null;
            var result = new MusicQueryData()
            {
                Id = music.Id,
                ContractAddress = music.ContractAddress,
                DateCreated = music.DateCreated,
                MusicLink = music.LicenceLink,
                Name = music.Name,
                Title = music.Title,
                PublishingYear = music.PublishingYear,
                Album = music.Album,
                TransactionHash = music.TransactionHash,
                TransactionStatus = music.TransactionStatus.ToString(),
                CreatureType = music.CreatureType.ToString(),
                OwnerType = music.OwnerType.ToString(),
                OwnerId = music.OwnerId
            };
            return result;
        }
    }
}
