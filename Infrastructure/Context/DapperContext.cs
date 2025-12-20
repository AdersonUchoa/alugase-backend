using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Context
{
    public class DapperContext
    {
        private SqlConnection? _connection;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");
        }

        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            if (_connection is not null && _connection.State == ConnectionState.Open)
                return _connection;

            _connection = new SqlConnection(_connectionString);
            _connection.Open();
            return _connection;
        }

        public void ReleaseConnection()
        {
            if (_connection is null)
                throw new InvalidOperationException("Connection is null.");

            if (_connection.State is ConnectionState.Closed or ConnectionState.Broken)
                throw new InvalidOperationException("Connection is already closed or broken.");

            if (_connection.State is not ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}