using BlockchainExplorer.DTO.Models.Common;
using BlockchainExplorer.Models.Common;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace BlockChainExplorer.SQLServer.Common
{
    public class BlockchainDbContext : DbContext
    {
        public BlockchainDbContext(DbContextOptions<BlockchainDbContext> options) : base(options)
        {
        }

        public DbSet<Network> Networks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}