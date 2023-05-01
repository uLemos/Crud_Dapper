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
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConector _dbConector;

        public OrderRepository(IDbConector dbConector)
        {
            this._dbConector = dbConector;
        }

        const string baseSql = @"SELECT [Id]
                                  ,[ClientId]
                                  ,[UserId]
                                  ,[CreateAt]
                                FROM [dbo].[Orders]
                            WHERE 1 = 1";
        public async Task ClientItemAsync(OrderItemModel item)
        {
            const string sql = @"INSERT INTO [dbo].[OrderItem]
                                   ([Id]
                                   ,[OrderId]
                                   ,[ProductId]
                                   ,[SellValue]
                                   ,[Quantity]
                                   ,[TotalAmount]
                                   ,[CreateAt])
                             VALUES
                                   (@Id
                                   ,@OrderIf
                                   ,@ProductId
                                   ,@SellValue
                                   ,@Quantity
                                   ,@TotalAmount
                                   ,@CreateAt)";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductId = item.ProductId, 
                SellValue = item.SellValue,
                Quantity = item.Quantity,
                TotalAmount = item.TotalAmount,
                CreateAt = item.CreateAt

            }, _dbConector.dbTransaction);
        }

        public async Task CreateAsync(OrderModel order)
        {
            const string sql = @"INSERT INTO [dbo].[Orders]
                                   ([Id]
                                   ,[ClientId]
                                   ,[UserId]
                                   ,[CreateAt])
                             VALUES
                                   (@Id,
                                   @ClientId,
                                   @UserId
                                   @CreateAt)";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Client = order.Client,
                User = order.User,
                Items = order.Items,

            }, _dbConector.dbTransaction);
        }

        public async Task UpdateAsync(OrderModel order)
        {
            const string sql = @"UPDATE [dbo].[Orders]
                                   SET [Id] = <Id, varchar(32),>
                                      ,[ClientId] = <ClientId, varchar(32),>
                                      ,[UserId] = <UserId, varchar(32),>
                                      ,[CreateAt] = <CreateAt, datetime2(7),>
                                 WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                Client = order.Client,
                User = order.User,
                Items = order.Items,

            }, _dbConector.dbTransaction);

        }

        public async Task UpdateItemAsync(OrderItemModel item)
        {
            const string sql = @"UPDATE [dbo].[OrderItem]
                                   SET [Id] = <Id, varchar(32),>
                                      ,[OrderId] = <OrderId, varchar(32),>
                                      ,[ProductId] = <ProductId, varchar(32),>
                                      ,[SellValue] = <SellValue, numeric(8,2),>
                                      ,[Quantity] = <Quantity, int,>
                                      ,[TotalAmount] = <TotalAmount, numeric(8,2),>
                                      ,[CreateAt] = <CreateAt, datetime2(7),>
                                 WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new
            {
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                SellValue = item.SellValue,
                Quantity = item.Quantity,
                TotalAmount = item.TotalAmount,

            }, _dbConector.dbTransaction);
        }

        public async Task DeleteAsync(string orderId)
        {
            string sql = @"DELETE FROM [dbo].[Orders] WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new { Id = orderId }, _dbConector.dbTransaction);
        }

        public async Task DeleteItemAsync(string itemId)
        {
            string sql = @"DELETE FROM [dbo].[OrderItem] WHERE Id = @Id";

            await _dbConector.dbConnection.ExecuteAsync(sql, new { Id = itemId }, _dbConector.dbTransaction);
        }

        public async Task<bool> ExistByIdAsync(string orderId)
        {
            string sql = $"{ baseSql}";

            var orders = await _dbConector.dbConnection.QueryAsync<bool>(sql, new { Id = orderId }, _dbConector.dbTransaction);

            return orders.FirstOrDefault();
        }

        public async Task<OrderModel> GetByIdAsync(string orderId)
        {
            string sql = $"{baseSql} AND Id = @Id";

            var orders = await _dbConector.dbConnection.QueryAsync<OrderModel>(sql, new { Id = orderId }, _dbConector.dbTransaction);

            return orders.FirstOrDefault();
        }

        public async Task<List<OrderModel>> ListByFilterAsync(string orderId = null, string name = null)
        {
            string sql = $"{baseSql}";

            if (string.IsNullOrWhiteSpace(orderId))
                sql += "AND Id = @Id";

            if (string.IsNullOrWhiteSpace(name))
                sql += "AND Name like @Name";

            var orders = await _dbConector.dbConnection.QueryAsync<OrderModel>(sql, new { Id = orderId, Name = "%" + name + "%" }, _dbConector.dbTransaction);
            
            return orders.ToList();
        }

        public async Task<List<OrderItemModel>> ListItemOrderIdAsync(string orderId)
        {
            string sql = $"{baseSql}";

            var orders = await _dbConector.dbConnection.QueryAsync<OrderItemModel>(sql, new { Id = orderId }, _dbConector.dbTransaction);

            return orders.ToList();
        }
    }
}
