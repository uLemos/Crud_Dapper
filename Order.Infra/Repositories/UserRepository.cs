using Dapper;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.Repositories.DataConector;
using Order.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Order.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConector _dbConector;

        public UserRepository(IDbConector dbConector)
        {
            _dbConector = dbConector;
        }

        const string baseSql = @"SELECT [Id]
                                  ,[Name]
                                  ,[Login]
                                  ,[PasswordHash]
                                  ,[CreateAt]
                                FROM [dbo].[Users]
                            WHERE 1 = 1";

        public async Task CreateAsync(UserModel user)
        {
            const string sql = @"INSERT INTO [dbo].[Users]
                                   ([Id]
                                   ,[Name]
                                   ,[Login]
                                   ,[PasswordHash]
                                   ,[CreateAt])
                             VALUES
                                   (@Id,
                                   @Name,,<Name,
                                   @Login,<Login,
                                   @PasswordHash,
                                   @Creatat)";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Name = user.Id,
                Login = user.Name,
                PasswordHash = user.PasswordHash,

            }, _dbConector.dbTransaction);
        }

        public async Task UpdateAsync(UserModel user)
        {
            const string sql = @"UPDATE [dbo].[Users]
                                   SET [Id] = <Id, varchar(32),>
                                      ,[Name] = <Name, varchar(100),>
                                      ,[Login] = <Login, varchar(20),>
                                      ,[PasswordHash] = <PasswordHash, varchar(max),>
                                      ,[CreateAt] = <CreateAt, datetime2(7),>
                                 WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Name = user.Name,
                Login = user.Login,
                PasswordHash = user.PasswordHash,

            }, _dbConector.dbTransaction);
        }

        public async Task DeleteAsync(string userId)
        {
            string sql = @"DELETE FROM [dbo].[Users] WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new { Id = userId }, _dbConector.dbTransaction);
        }

        public async Task<bool> ExistByIdAsync(string userId)
        {
            string sql = $"{baseSql}";

            var users = await _dbConector.dbConnection.QueryAsync<bool>(sql, new { Id = userId }, _dbConector.dbTransaction);

            return users.FirstOrDefault();
        }

        public async Task<bool> ExistByLoginAsync(string loginId)
        {
            string sql = $"{baseSql}";

            var logins = await _dbConector.dbConnection.QueryAsync<bool>(sql, new { Id = loginId }, _dbConector.dbTransaction);

            return logins.FirstOrDefault();
        }

        public async Task<UserModel> GetByIdAsync(string userId)
        {
            string sql = $"{baseSql} AND Id = @Id";

            var users = await _dbConector.dbConnection.QueryAsync<UserModel>(sql, new { Id = userId }, _dbConector.dbTransaction);

            return users.FirstOrDefault();
        }

        public async Task<List<UserModel>> ListByFilterAsync(string userId = null, string name = null)
        {
            string sql = $"{baseSql}";

            if (string.IsNullOrWhiteSpace(userId))
                sql += "AND Id = @Id";

            if (string.IsNullOrWhiteSpace(name))
                sql += "AND Name like @Name";

            var users = await _dbConector.dbConnection.QueryAsync<UserModel>(sql, new { Id = userId, Name = "%" + name + "%" }, _dbConector.dbTransaction);

            return users.ToList();
        }
    }
}
