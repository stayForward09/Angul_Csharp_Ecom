using Microsoft.EntityFrameworkCore;
using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class PartRepository : GenericRepository<Part>, IPartRepository
{
    public PartRepository(PartDbContext context, ILogger logger) : base(context, logger)
    {

    }

    public async Task<object> SearchbyText(string searchText)
    {
        try
        {
            var data = await dbSet.Where(x => x.PartName.ToLower().Contains(searchText.ToLower())).Select(x => new
            {
                prd = x.Pid,
                prdName = x.PartName
            }).OrderBy(x => x.prdName).Take(10).ToListAsync();
            return data;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Repo} SearchbyText method error", typeof(UserRepository));
            return null;
        }
    }
}