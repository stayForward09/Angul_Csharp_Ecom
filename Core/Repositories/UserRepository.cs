using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StackApi.Core.IRepositories;
using StackApi.Core.Repositories;
using StackApi.Data;
using StackApi.Dtos;
using StackApi.Models;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(PartDbContext context, ILogger logger) : base(context, logger)
    {

    }

    public override async Task<IEnumerable<User>> All()
    {
        try
        {
            return await dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Repo} all method error", typeof(UserRepository));
            return new List<User>();
        }
    }
    public override async Task<bool> Add(User Entity)
    {
        try
        {
            await dbSet.AddAsync(Entity);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Repo} Add method error", typeof(UserRepository));
            return false;
        }
    }

    public async Task<User> CheckEmailExists(string mailid)
    {
        try
        {
            return await dbSet.FirstOrDefaultAsync(x => x.EmailID == mailid);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Repo} Add method error", typeof(UserRepository));
            return null;
        }
    }
    public override async Task<bool> Update(User Entity)
    {
        try
        {
            var data = await context.Users.FirstOrDefaultAsync(x => x.UsID == Entity.UsID);
            if (data == null)
            {
                return false;
            }
            context.Users.Update(Entity);
            var affectedRows = await context.SaveChangesAsync();
            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Repo} Update method error", typeof(UserRepository));
            return false;
        }
    }

    public async Task<User> GetUserbyCondition(Expression<Func<User, bool>> condition, bool disableTracking = true)
    {
        if (disableTracking)
            context.Users.AsNoTracking();
            
        return await context.Users.FirstOrDefaultAsync(condition);
    }
}