using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class OrderRespository : GenericRepository<Orders>, IOrdersRepositories
{
    public OrderRespository(PartDbContext partDbContext, ILogger logger) : base(partDbContext, logger)
    {

    }
}