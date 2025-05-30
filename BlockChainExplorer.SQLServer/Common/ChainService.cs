using BlockchainExplorer.Models.Common;
using BlockChainExplorer.SQLServer.Common.Interface;
using System.Data.SqlClient;

namespace BlockChainExplorer.SQLServer.Common
{
    public class ChainService : IChainService
    {
        private readonly BlockchainDbContext _context;

        public ChainService(BlockchainDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNetwork(Network network)
        {
            _context.Networks.Add(network);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNetwork(Network network)
        {
            _context.Networks.Update(network);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNetwork(int id)
        {
            var network = await _context.Networks.FindAsync(id);
            if (network != null)
            {
                _context.Networks.Remove(network);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool IsExistNetwork(int chainIndex)
        {
            throw new NotImplementedException();
        }

        //public async Task<int> ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters)
        //{
        //    return await _context.Networks.FromSql(procedureName).ToListAsync();
        //}
    }
}
