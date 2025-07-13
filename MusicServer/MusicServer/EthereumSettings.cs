using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer
{
    public class EthereumSettings
    {
        public EthereumSettings()
        {
            // Empty
        }
        public string EthereumAccount { get; set; }
        public string EthereumPassword { get; set; }


        /// <summary>
        /// Abi of the master contract.
        /// </summary>
        public string Abi { get; set; }
        public string ByteCode { get; set; }
        /// <summary>
        /// Address of the master contract on Ethereum blockchain.
        /// </summary>
        public string ContractAddress { get; set; }

        public string MusicAbi { get; set; }
        public string MusicAssetTransferAbi { get; set; }
    }
}
