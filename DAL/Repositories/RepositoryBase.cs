using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly ApplicationContext context;
        //private readonly DbSet<TEntity> dbSet;
        public RepositoryBase(ApplicationContext context)
        {
            this.context = context;
            //this.dbSet = context.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            context.Set<TEntity>().AddAsync(item);
            Save();
        }

        public void Delete(TEntity item)
        {
            context.Set<TEntity>().Remove(item);
            Save();
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> match)
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(match);
        }

        public void Update(TEntity item)
        {
            context.Set<TEntity>().Update(item);
            Save();
        }
        public void Save()
        {
            context.SaveChangesAsync();
        }
    }
}
