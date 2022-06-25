using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StackApi.Core.IRepositories;
using StackApi.Data;

namespace StackApi.Core.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected PartDbContext context;
    protected DbSet<T> dbSet;
    protected readonly ILogger logger;

    public GenericRepository(PartDbContext _context, ILogger _logger)
    {
        context = _context;
        logger = _logger;
        dbSet = context.Set<T>();
    }
    public virtual async Task<bool> Add(T Entity)
    {
        await dbSet.AddAsync(Entity);
        return true;
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await dbSet.ToListAsync();
    }

    public virtual Task<bool> Delete(Guid ID)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<T> GetByID(Guid ID)
    {
        return await dbSet.FindAsync(ID);
    }

    public virtual Task<bool> Update(T Entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> getByCondition(Expression<Func<T, bool>> predicate)
    {
        var data = await dbSet.Where(predicate).ToListAsync();
        return data;
    }

    public async Task<T> getFirstByCondition(Expression<Func<T, bool>> predicate)
    {
        var data = await dbSet.FirstOrDefaultAsync(predicate);
        return data;
    }
}