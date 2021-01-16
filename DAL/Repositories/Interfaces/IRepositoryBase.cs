﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Get();
        Task<TEntity> Get(Expression<Func<TEntity, bool>> match);
        void Create(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        void Save();
    }
}
