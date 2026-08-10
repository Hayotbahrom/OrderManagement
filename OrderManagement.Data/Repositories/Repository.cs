using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data.Contexts;

namespace OrderManagement.Data.Reposiroty;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> Set;

    public Repository(AppDbContext context)
    {
        Context = context;
        Set = context.Set<TEntity>();
    }

    public IQueryable<TEntity> SelectAll()
        => Set.AsNoTracking();

    public async Task<TEntity?> SelectByIdAsync(int id)
        => await Set.FindAsync(id);

    public async Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> predicate)
        => await Set.AsNoTracking().FirstOrDefaultAsync(predicate);

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        Set.Update(entity);
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await Set.FindAsync(id);
        if (entity is not null)
            Set.Remove(entity);
    }

    public async Task SaveChangesAsync()
        => await Context.SaveChangesAsync();
}