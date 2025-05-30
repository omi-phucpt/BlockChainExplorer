using BlockchainExplorer.Models.Common;
using System.Data.SqlClient;

namespace BlockChainExplorer.SQLServer.Common.Interface
{
    internal interface IChainService
    {
        Task<bool> CreateNetwork(Network network);
        Task<bool> UpdateNetwork(Network network);
        Task<bool> DeleteNetwork(int id);
        bool IsExistNetwork(int chainIndex);
    }
}
