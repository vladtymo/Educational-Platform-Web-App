using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task Save();

        //Task<IEnumerable<TEntity>> Get(
        //    Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    string includeProperties = "");
        Task<TEntity?> GetItemBySpec(ISpecification<TEntity> specification);
        Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification);

        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> GetByID(object id);

        Task Insert(TEntity entity);

        Task Delete(object id);

        Task Delete(TEntity entityToDelete);

        Task Update(TEntity entityToUpdate);

    }
}
