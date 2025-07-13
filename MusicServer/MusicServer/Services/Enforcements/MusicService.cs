using MusicServer.Repositories.Interfaces;
using MusicServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using MusicServer.Utilities;
using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using Hangfire;
using MusicServer.UndergroundJobs.Interfaces;

namespace MusicServer.Services.Enforcements
{
    public class MusicService : IMusicService
    {
        private readonly IEthereumService ethereumService;
        private readonly IMusicRepository musicRepository;
        private readonly string MusicAbi;

        public MusicService(
            IEthereumService ethereumService,
            IMusicRepository musicRepository,
            IOptions<EthereumSettings> options)
        {
            this.ethereumService = ethereumService;
            this.musicRepository = musicRepository;
            MusicAbi = options.Value.MusicAbi;
        }

        async Task<Guid> IMusicService.Create(
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
            OwnerTypes ownerType)
        {
            
            try
            {
                var music = new MusicInfo()
                {
                    Name = name,
                    Album = album,
                    PublishingYear = publishingYear,
                    OwnerId = ownerId,
                    LicenceLink = licenceLink,
                    MusicLink = musicLink,
                    DemoLink = demoLink,
                    Key1 = key1,
                    Key2 = key2,
                    FullKey = fullKey,
                    CreatureType = creatureType,
                    OwnerType = ownerType,
                    DateCreated = DateTime.UtcNow
                };
                Guid newMusicId = musicRepository.CreateAndReturnId(music);

                var deployContract = ethereumService.DeployContract();

                bool isCreatedContract = false;
                do
                {
                    var receipt = deployContract.Result;
                    if (receipt == null)
                        continue;
                    if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                    {
                        isCreatedContract = true;

                        var function = ethereumService.GetFunction(EthereumFunctions.AddMusicAsset);
                        var transactionHash = await function.SendTransactionAsync(
                        ethereumService.GetEthereumAccount(),
                        new HexBigInteger(4000000),
                        new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                        new HexBigInteger(0),
                        functionInput: new object[] {
                            newMusicId.ToString(),
                            name,
                            album,
                            publishingYear,
                            key2,
                            ownerId,
                            licenceLink,
                            musicLink,
                            (uint)creatureType,
                            (uint)ownerType
                        });

                        music.ContractAddress = ethereumService.GetMasterContractAddress();
                        music.TransactionHash = transactionHash;
                        musicRepository.Update(music);

                    }
                }
                while (isCreatedContract != true);

                BackgroundJob.Schedule<IMusicUndergroundJob>(
                    backgroundJob => backgroundJob.WaitForTransactionToSuccessThenFinishCreatingMusic(music),
                    TimeSpan.FromSeconds(3)
                );

                return newMusicId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task IMusicService.UpdateOwnerForChangeOwnerShip(Guid id, uint ownerId, string musicLink)
        {
            var music = musicRepository.Get(id);
            if (music == null)
            {
                throw new ArgumentException("Id does not exist in the system.", nameof(id));
            }

            var deployContract = ethereumService.DeployContract();

            bool isCreatedContract = false;
            do
            {
                var receipt = deployContract.Result;
                if (receipt == null)
                    continue;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    isCreatedContract = true;

                    var function = ethereumService.GetFunction(EthereumFunctions.AddMusicAsset);
                    var transactionHash = await function.SendTransactionAsync(
                    ethereumService.GetEthereumAccount(),
                    new HexBigInteger(4000000),
                    new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                    new HexBigInteger(0),
                    functionInput: new object[] {
                            music.Id.ToString(),
                            music.Name.ToString(),
                            music.Album.ToString(),
                            music.PublishingYear.ToString(),
                            music.Key2.ToString(),
                            ownerId,
                            music.LicenceLink.ToString(),
                            musicLink,
                            (uint)music.CreatureType,
                            (uint)music.OwnerType
                    });

                    music.ContractAddress = ethereumService.GetMasterContractAddress();
                    music.TransactionHash = transactionHash;
                    music.OwnerId = ownerId;
                    music.MusicLink = musicLink;
                    musicRepository.Update(music);
                }
            }
            while (isCreatedContract != true);

            BackgroundJob.Schedule<IMusicUndergroundJob>(
                backgroundJob => backgroundJob.WaitForTransactionToSuccessThenFinishCreatingMusic(music),
                TimeSpan.FromSeconds(3)
            );

        }

        void IMusicService.UpdateKey(
            Guid musicId,
            string key1,
            string fullKey,
            uint ownerId,
            string musicLink)
        {
                  
            musicRepository.UpdateKey(musicId, key1, fullKey, ownerId, musicLink);

        }

        void IMusicService.CreateMusicOwnerShip(Guid musicId, int userId)
        {
            var share = new ShareOwnerShip()
            {
                MusicId = musicId,
                UserId = userId
            };
            musicRepository.CreateMusicOwnerShip(share);
        }

        List<MusicQueryData> IMusicService.GetAllMusics()
        {
            try
            {
                var musics = musicRepository.GetAll();
                List<MusicQueryData> result = new List<MusicQueryData>();
                foreach (var music in musics)
                {
                    result.Add(music.ToMusicQueryData());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        MusicInfo IMusicService.GetMusicWithId(Guid musicId)
        {
            try
            {
                var music = musicRepository.Get(musicId);
                return music;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ShareOwnerShip IMusicService.GetMusicWithIdAndUserId(Guid musicId, int userId)
        {
            try
            {
                var share = musicRepository.GetMusicWithIdAndUserId(musicId, userId);

                return share;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<MusicQueryData> IMusicService.GetMusicListWithUserID(uint userID)
        {
            try
            {
                var musics = musicRepository.GetWithUserID(userID);
                return musics;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<MusicQueryData> IMusicService.GetMusicListWithUserIDWithBuying(uint userID)
        {
            try
            {
                var musics = new List<MusicQueryData>();
                var musicTF = musicRepository.GetWithUserIDWithTFBuying(userID);
                foreach (var item in musicTF)
                {
                    musics.Add(musicRepository.GetWithUserIDWithBuying(item.MusicId, item.MediaLink, item.Id, item.IsPermanent, item.IsConfirmed));
                }
                return musics;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<MusicQueryData> IMusicService.GetMusicShareOwnerShip(int userID)
        {
            try
            {
                var musics = musicRepository.GetMusicShareOwnerShip(userID);
                return musics;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<int> IMusicService.GetOwnerBuyerId(Guid musicId)
        {
            try
            {
                var ownerIdList = new List<int>();
                var ownerList = musicRepository.GetOwnerId(musicId);
                foreach (var item in ownerList)
                {
                    ownerIdList.Add(Convert.ToInt32(item.OwnerId));
                }

                var buyerIdList = new List<int>();
                var buyerList = musicRepository.GetBuyerId(musicId);
                foreach (var item in buyerList)
                {
                    buyerIdList.Add(item.BuyerId);
                }

                var ownerBuyerIdList = new List<int>();
                ownerBuyerIdList = ownerIdList.Union(buyerIdList).ToList();

                return ownerBuyerIdList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<MusicInfoSC> IMusicService.GetMusicAsset(int ownerId, Guid id)
        {
            var music = musicRepository.Get(id);
            ethereumService.SetMasterContractAddress(music.ContractAddress);
            var function = ethereumService.GetFunction("getMusicAsset");
            try
            {
                var result = await function.CallDeserializingToObjectAsync<MusicInfoSC>(ownerId.ToString(), id.ToString());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<MusicInfoSC> IMusicService.GetMusicAssetForOrigin(string transactionHash)
        {
            var music = musicRepository.GetMusicWithTransactionHash(transactionHash);
            ethereumService.SetMasterContractAddress(music.ContractAddress);
            var function = ethereumService.GetFunction("getMusicAsset");
            try
            {
                var result = await function.CallDeserializingToObjectAsync<MusicInfoSC>(music.OwnerId.ToString(), music.Id.ToString());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<MusicTransferSC> IMusicService.GetMusicTransferForOrigin(string transactionHash)
        {
            var musicTF = musicRepository.GetMusicTFWithTransactionHash(transactionHash);
            ethereumService.SetMasterContractAddress(musicTF.ContractAddress);
            var function = ethereumService.GetFunction("getMusicAssetTransfer");
            try
            {
                var result = await function.CallDeserializingToObjectAsync<MusicTransferSC>(musicTF.Id.ToString());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        MusicInfo IMusicService.GetMusicWithTransactionHash(string transactionHash)
        {
            try
            {
                var share = musicRepository.GetMusicWithTransactionHash(transactionHash);

                return share;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        MusicAssetTransfer IMusicService.GetMusicTFWithTransactionHash(string transactionHash)
        {
            try
            {
                var share = musicRepository.GetMusicTFWithTransactionHash(transactionHash);

                return share;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
