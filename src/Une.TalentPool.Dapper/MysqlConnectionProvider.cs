using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Une.TalentPool.Dapper
{
    public class MysqlConnectionProvider : IConnectionProvider
    {
        private bool _disposed;
        private DbConnection _connection;

        public MysqlConnectionProvider(IOptions<DapperOptions> options)
        {
            Options = options?.Value;
        }
        protected DapperOptions Options { get; }

        public async Task<DbConnection> GetDbConnectionAsync()
        {
            _connection = new MySqlConnection(Options.ConnectionString);
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();
            return _connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _connection?.Dispose();
            }
            _disposed = true;
        }
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
