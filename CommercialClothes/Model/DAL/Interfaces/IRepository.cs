﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        IQueryable<T> GetQuery(Expression<Func<T, bool>> expression);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        Task Delete(Expression<Func<T, bool>> expression);
        void Delete(IEnumerable<T> entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
