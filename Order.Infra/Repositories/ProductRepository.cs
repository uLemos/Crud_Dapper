using Dapper;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.Repositories.DataConector;
using Order.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Order.Infra.Repositories
{
    public class ProductRepository : IProductRespository
    {
        private readonly IDbConector _dbConector;

        public ProductRepository(IDbConector dbConector)
        {
            _dbConector = dbConector;
        }

        const string baseSql = @"SELECT [Id]
                                  ,[Description]
                                  ,[SellValue]
                                  ,[Stock]
                                  ,[CreateAt]
                                FROM [dbo].[Product]
                            WHERE 1 = 1";

        public async Task CreateAsync(ProductModel product)
        {
            const string sql = @"INSERT INTO [dbo].[Product]
                               ([Id]
                               ,[Description]
                               ,[SellValue]
                               ,[Stock]
                               ,[CreateAt])
                         VALUES
                                (@Id,
                                @Description,
                                @SellValue,
                                @Stock,
                                @CreateAt)";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Description = product.Description,
                SellValue = product.SellValue,
                Stock = product.Stock,

            }, _dbConector.dbTransaction);
        }

        public async Task UpdateAsync(ProductModel product)
        {
            const string sql = @"UPDATE [dbo].[Product]
                                   SET [Id] = <Id, varchar(32),>
                                      ,[Description] = <Description, varchar(100),>
                                      ,[SellValue] = <SellValue, numeric(8,2),>
                                      ,[Stock] = <Stock, int,>
                                      ,[CreateAt] = <CreateAt, datetime2(7),>
                                 WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Description = product.Description,
                SellValue = product.SellValue,
                Stock = product.Stock,

            }, _dbConector.dbTransaction);
        }

        public async Task DeleteAsync(string productId)
        {
            string sql = @"DELETE FROM [dbo].[Product] WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new { Id = productId }, _dbConector.dbTransaction);
        }

        public async Task<bool> ExistByIdAsync(string productId)
        {
            string sql = $"{ baseSql}";

            var products = await _dbConector.dbConnection.QueryAsync<bool>(sql, new { Id = productId }, _dbConector.dbTransaction);

            return products.FirstOrDefault();
        }

        public async Task<ProductModel> GetByIdAsync(string productId)
        {
            string sql = $"{baseSql} AND Id = @Id";

            var products = await _dbConector.dbConnection.QueryAsync<ProductModel>(sql, new { Id = productId }, _dbConector.dbTransaction);

            return products.FirstOrDefault();
        }

        public async Task<List<ProductModel>> ListByFilterAsync(string productId = null, string description = null)
        {
            string sql = $"{baseSql}";

            if (string.IsNullOrWhiteSpace(productId))
                sql += "AND Id = @Id";

            if (string.IsNullOrWhiteSpace(description))
                sql += "AND Description like @Description";

            var products = await _dbConector.dbConnection.QueryAsync<ProductModel>(sql, new { Id = productId, Description = description }, _dbConector.dbTransaction);

            return products.ToList();
        }
    }
}
