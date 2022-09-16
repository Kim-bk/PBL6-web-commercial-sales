using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComercialClothes.Models.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
