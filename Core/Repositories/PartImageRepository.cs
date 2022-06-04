using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class PartImageRepository : GenericRepository<PartImages>,IPartImageRepository
{
    public PartImageRepository(PartDbContext context, ILogger logger) : base(context, logger)
    {

    }
}