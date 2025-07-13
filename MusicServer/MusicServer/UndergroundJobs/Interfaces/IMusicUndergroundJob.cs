using MusicServer.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.UndergroundJobs.Interfaces
{
    public interface IMusicUndergroundJob
    {
        void WaitForTransactionToSuccessThenFinishCreatingMusic(MusicInfo music);

        void SyncDatabaseWithBlockchain();
    }
}
