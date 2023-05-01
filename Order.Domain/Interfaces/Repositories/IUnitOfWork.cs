using Order.Domain.Interfaces.Repositories.DataConector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IClientRepository ClientRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRespository ProductRepository { get; }
        IUserRepository UserRepository { get; }


        public IDbConector dbConector { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();

    }
}
