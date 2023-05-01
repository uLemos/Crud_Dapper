using System;
using System.Data;
 
namespace Order.Domain.Interfaces.Repositories.DataConector
{
    public interface IDbConector : IDisposable
    {
        IDbConnection dbConnection { get; }
        IDbTransaction dbTransaction { get; set; }
    }
}
