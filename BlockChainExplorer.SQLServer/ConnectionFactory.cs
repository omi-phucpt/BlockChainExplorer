using Microsoft.Data.SqlClient;

namespace BlockChainExplorer.SQLServer
{
    public class ConnectionFactory
    {
        private readonly string _connectionString;
        public ConnectionFactory(IConfiguration config)
        {
            _connectionString = config["ConnectionDBStrings:StandardDB"];
        }
        public SqlConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
