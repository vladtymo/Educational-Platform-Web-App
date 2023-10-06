using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SS_EDUP.Core.Interfaces;
using SS_EDUP.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SS_EDUP.Infrastructure.Repository
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        internal AppDbContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(AppDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        //public async virtual Task< IEnumerable<TEntity> >Get(
        //    Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    string includeProperties = "")
        //{
        //    IQueryable<TEntity> query = dbSet;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    foreach (var includeProperty in includeProperties.Split
        //        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    if (orderBy != null)
        //    {
        //        return await orderBy(query).ToListAsync();
        //    }
        //    else
        //    {
        //        return await query.ToListAsync();
        //    }
        //}


        public async virtual Task<TEntity?> GetByID(object id)
        {
           return await dbSet.FindAsync(id);
        }

        public async virtual Task Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async virtual Task Delete(object id)
        {
            TEntity? entityToDelete = await dbSet.FindAsync(id);
            if(entityToDelete!=null)
                await  Delete(entityToDelete);
        }

        public  async virtual Task Delete(TEntity entityToDelete)
        {
            await  Task.Run(
                () =>
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);
            });
        }

        public async virtual Task Update(TEntity entityToUpdate)
        {
            await Task.Run(
                () =>
                {
                    dbSet.Attach(entityToUpdate);
                    context.Entry(entityToUpdate).State = EntityState.Modified;
                });
            
        }

        public async Task<TEntity?> GetItemBySpec(ISpecification<TEntity> specification)
        {
            // apply specification
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(dbSet, specification);
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }
    }
}
