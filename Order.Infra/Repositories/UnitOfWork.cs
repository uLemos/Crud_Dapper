using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.Repositories.DataConector;
using System;
using System.Data;

namespace Order.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IClientRepository _clientRepository;
        private IProductRespository _productRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;


        public UnitOfWork(IDbConector dbConector)
        {
            this.dbConector = dbConector;
        }

        public IClientRepository ClientRepository => _clientRepository ?? (_clientRepository = new ClientRepository(dbConector));

        public IOrderRepository OrderRepository => _orderRepository ?? (_orderRepository = new OrderRepository(dbConector));

        public IProductRespository ProductRepository => _productRepository ?? (_productRepository = new ProductRepository(dbConector));

        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(dbConector));

        public IDbConector dbConector { get; }

        public void BeginTransaction()
        {
            if (dbConector.dbConnection.State == System.Data.ConnectionState.Open)
                dbConector.dbTransaction = dbConector.dbConnection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);    
        }

        public void CommitTransaction()
        {
            if (dbConector.dbConnection.State == System.Data.ConnectionState.Open)
                dbConector.dbTransaction.Commit();
        }

        public void RollBackTransaction()
        {
            if (dbConector.dbConnection.State == System.Data.ConnectionState.Open)
                dbConector.dbTransaction.Rollback();
        }
    }
}
