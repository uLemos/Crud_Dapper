using Dapper;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.Repositories.DataConector;
using Order.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Order.Infra.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDbConector _dbConector;

        public ClientRepository(IDbConector dbConector)
        {
            _dbConector = dbConector;
        }

        const string baseSql = @"SELECT [Id]
                                ,[Name]
                                ,[Email]
                                ,[PhoneNumber]
                                ,[Adress]
                                ,[CreateAt]
                            FROM [dbo].[Client]
                            WHERE 1 = 1";

        public async Task CreateAsync(ClientModel client)
        {
            string sql = @"INSERT INTO [dbo].[Client]
                           ([Id]
                           ,[Name]
                           ,[Email]
                           ,[PhoneNumber]
                           ,[Adress]
                           ,[CreateAt])
                     VALUES
                            (@Id
                           ,@Name
                           ,@Email
                           ,@PhoneNumber
                           ,@Adress
                           ,@CreateAt)";



            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Adress = client.Adress,
                CreateAt = client.CreateAt

            }, _dbConector.dbTransaction);
        }

        public async Task UpdateAsync(ClientModel client)
        {
            string sql = @"UPDATE [dbo].[Client]
                           SET [Name] = @Name
                              ,[Email] = @Email
                              ,[PhoneNumber] = @PhoneNumber
                              ,[Adress] = @Adress
                         WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Adress = client.Adress,
                CreateAt = client.CreateAt
            }, _dbConector.dbTransaction);
        }

        public async Task DeleteAsync(string clientId)
        {
            string sql = @"DELETE FROM [dbo].[Client] WHERE id = @id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new { Id = clientId }, _dbConector.dbTransaction);
        }

        public async Task<bool> ExistByIdAsync(string clientId)
        {
            string sql = $"SELECT 1 FROM Client WHERER Id = @Id";

            var clients = await _dbConector.dbConnection.QueryAsync<bool>(sql, new { Id = clientId }, _dbConector.dbTransaction);

            return clients.FirstOrDefault();
        }

        public async Task<ClientModel> GetByIdAsync(string clientId)
        {
            string sql = $"{baseSql} AND Id = @Id";

            var clients = await _dbConector.dbConnection.QueryAsync<ClientModel>(sql, new { Id = clientId }, _dbConector.dbTransaction);

            return clients.FirstOrDefault();
        }

        public async Task<List<ClientModel>> ListByFilterAsync(string clientId = null, string name = null)
        {
            string sql = $"{baseSql}";

            if (string.IsNullOrWhiteSpace(clientId))
                sql += "AND Id = @Id";

            if (string.IsNullOrWhiteSpace(name))
                sql += "AND Name like @Name";

            var clients = await _dbConector.dbConnection.QueryAsync<ClientModel>(sql, new { Id = clientId, Name = "%" + name + "%" }, _dbConector.dbTransaction);

            return clients.ToList();
        }
    }
}
