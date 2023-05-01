using Dapper;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.Repositories.DataConector;
using Order.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Infra.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConector _dbConector;

        public OrderRepository(IDbConector dbConector)
        {
            this._dbConector = dbConector;
        }

        const string baseSql = @"SELECT o.[Id]
                                  ,o.[CreateAt]
                                  ,c.Id
                                  ,c.[Name]
                                  ,u.Id
                                  ,u.[Name]
                                FROM [dbo].[Orders]
                                JOIN [dbo].[Client] c ON o.ClientId = c.Id 
                                JOIN [dbo].[User] u ON o.UserId = u.Id
                            WHERE 1 = 1";
        public async Task CreateItemAsync(OrderItemModel item)
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
                Order = item.Order,
                Product = item.Product, 
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

            if (order.Items.Any())
            {
                foreach (var item in order.Items)
                {
                    await CreateItemAsync(item);
                }
            }
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
                Id = order.Id,
                ClientId = order.Client.Id,
                UserID = order.User.Id,
            }, _dbConector.dbTransaction);

            if (order.Items.Any())
            {
                string deleteItem = @"DELETE FROM [dbo].[OrderItem] WHERE OrderId = @OrderId";

                await _dbConector.dbConnection.ExecuteAsync(deleteItem, new { OrderId = order.Id }, _dbConector.dbTransaction);

                foreach (var item in order.Items)
                {
                    await CreateItemAsync(item);
                }
            }
        }

        //public async Task UpdateItemAsync(OrderItemModel item)
        //{
        //    const string sql = @"UPDATE [dbo].[OrderItem]
        //                           SET [Id] = <Id, varchar(32),>
        //                              ,[OrderId] = <OrderId, varchar(32),>
        //                              ,[ProductId] = <ProductId, varchar(32),>
        //                              ,[SellValue] = <SellValue, numeric(8,2),>
        //                              ,[Quantity] = <Quantity, int,>
        //                              ,[TotalAmount] = <TotalAmount, numeric(8,2),>
        //                              ,[CreateAt] = <CreateAt, datetime2(7),>
        //                         WHERE Id = @Id";

        //    await _dbConector.dbConnection.ExecuteAsync(sql, new
        //    {
        //        Order = item.Order,
        //        Product = item.Product,
        //        SellValue = item.SellValue,
        //        Quantity = item.Quantity,
        //        TotalAmount = item.TotalAmount,

        //    }, _dbConector.dbTransaction);
        //}

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
                                                                                                       //Retorno
            var orders = await _dbConector.dbConnection.QueryAsync<OrderModel, ClientModel, UserModel, OrderModel>(
                //Divisões para o mapeamento
                sql: sql,                           //Caminho da Query que será usada
                map: (order, client, user) => {     //Nomeação de cada objeto sendo usado no "QuerAsync"
                    order.Client = client;          //Mapeamento
                    order.User = user;
                    return order;
                },
                param: new { Id = orderId },        //Id, como escolha de retorno de novo objeto.
                splitOn: "Id",                      ////Sempre que bater em um Id, irá retornar um novo objeto.                  
                transaction: _dbConector.dbTransaction);

            return orders.FirstOrDefault();
        }

        public async Task<List<OrderModel>> ListByFilterAsync(string orderId = null, string clientId = null, string userId = null)
        {
            string sql = $"{baseSql}";

            if (string.IsNullOrWhiteSpace(orderId))
                sql += "AND Id = @Id";

            if (string.IsNullOrWhiteSpace(clientId))
                sql += "AND Id = @Id";

            if (string.IsNullOrWhiteSpace(userId))
                sql += "AND Name like @Name";

            var orders = await _dbConector.dbConnection.QueryAsync<OrderModel, ClientModel, UserModel, OrderModel>(
                sql: sql,
                map: (order, client, user) =>
                {
                    order.Client = client;
                    order.User = user;
                    return order;
                },
                param: new { OrderId = orderId, ClientId = clientId, UserId = userId },
                splitOn: "Id",
                transaction: _dbConector.dbTransaction);

            return orders.ToList();
        }

        public async Task<List<OrderItemModel>> ListItemOrderIdAsync(string orderId)
        {
            string sql = @"SELECT oi.[Id]
                                 ,oi.[OrderId]
                                ,oi.[ProductId]
                                ,oi.[SellValue]
                                ,oi.[Quantity]
                                ,oi.[TotalAmount]
                                ,oi.[CreateAt]
                                ,p.[Description]
                            FROM [dbo].[OrderItem]
                            JOIN [dbo].[Product] p on oi.ProductId = p.id
                            WHERE oi.OrderId = @OrderId";

            var itens = await _dbConector.dbConnection.QueryAsync<OrderItemModel, OrderModel, ProductModel, OrderItemModel>(
                sql: sql,
                map: (item, order, prod) =>
                {
                    item.Order = order;
                    item.Product = prod;
                    return item;
                },
                param: new { Id = orderId },
                splitOn: "Id",
                transaction: _dbConector.dbTransaction);

            return itens.ToList();
        }
    }
}
