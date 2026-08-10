using System.Linq.Expressions;

namespace OrderManagement.Data.Reposiroty;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> SelectAll();
    Task<TEntity?> SelectByIdAsync(int id);
    Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> InsertAsync(TEntity entity);
    TEntity Update(TEntity entity);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}