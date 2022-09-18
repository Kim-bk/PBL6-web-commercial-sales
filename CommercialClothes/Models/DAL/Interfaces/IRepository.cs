using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ComercialClothes.Models.DAL
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        void DeleteExp(Expression<Func<T, bool>> expression);
        void Update(T entity);
        void Delete(T entity);
    }
}
