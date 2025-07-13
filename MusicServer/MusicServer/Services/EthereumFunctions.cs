using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Services
{
    public class EthereumFunctions
    {
        public static readonly string AddMusicAsset = "addMusicAsset";
        public static readonly string UpdateMusicInformation = "updateMusicInformation";
        public static readonly string RemoveMusic = "removeMusic";

        public static readonly string AddTransactionMusic = "addTransactionMusic";
        public static readonly string UpdateMusicAssetTransfer = "updateMusicAssetTransfer";
        public static readonly string RemoveMusicAssetTransfer = "removeMusicAssetTransfer";

        public static readonly string GetMusicAssetTransfer = "getMusicAssetTransfer";

        public static readonly string SelfDelete = "selfDelete";
    }
}
