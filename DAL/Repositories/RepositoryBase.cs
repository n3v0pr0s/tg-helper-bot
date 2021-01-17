using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly ApplicationContext context;
        public RepositoryBase(ApplicationContext context)
        {
            this.context = context;
        }

        public void Create(TEntity item)
        {
            context.Set<TEntity>().AddAsync(item);
            context.SaveChangesAsync();
        }

        public void Delete(TEntity item)
        {
            context.Set<TEntity>().Remove(item);
            context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> match)
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(match);
        }
        public async Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> match)
        {
            return await context.Set<TEntity>().Where(match).ToListAsync();
        }

        public void Update(TEntity item)
        {
            context.Set<TEntity>().Update(item);
            context.SaveChangesAsync();
        }

    }
}
