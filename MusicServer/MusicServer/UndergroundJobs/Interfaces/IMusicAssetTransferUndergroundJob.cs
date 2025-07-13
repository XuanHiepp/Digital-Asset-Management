using MusicServer.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.UndergroundJobs.Interfaces
{
    public interface IMusicAssetTransferUndergroundJob
    {
        void WaitForTransactionToSuccessThenFinishCreating(MusicAssetTransfer transfer);
    }
}
