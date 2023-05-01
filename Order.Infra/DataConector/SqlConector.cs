using Order.Domain.Interfaces.Repositories.DataConector;
using System.Data;
using System.Data.SqlClient;

namespace Order.Infra.DataConector
{
    public class SqlConector : IDbConector
    {
        public SqlConector(string connectionString)
        {
            dbConnection = SqlClientFactory.Instance.CreateConnection();
            dbConnection.ConnectionString = connectionString;
        }

        public IDbConnection dbConnection { get; }
        public IDbTransaction dbTransaction { get; set; }

        public void Dispose()
        {
            dbConnection?.Dispose();
            dbTransaction?.Dispose();
        }
    }
}
