using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;

namespace OrderManagement.Data.Reposiroty;

public interface IRepository<TEntity>
{
    Task<bool> DeleteAsync(int id);
    IQueryable<TEntity> SelectAll();
    Task<TEntity> SelectByIdAsync(int id);
    Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}
