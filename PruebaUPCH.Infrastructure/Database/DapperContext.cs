using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PruebaUPCH.Infrastructure.Database
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Conexion")
                              ?? throw new InvalidOperationException("Missing connection string.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
