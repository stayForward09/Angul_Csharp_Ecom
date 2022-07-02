using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class OrderRespository : GenericRepository<Orders>, IOrdersRepositories
{
    public OrderRespository(PartDbContext partDbContext, ILogger logger) : base(partDbContext, logger)
    {

    }

    public async Task<List<Orders>> getOrderbyCondition(Expression<Func<Orders, bool>> predicate, bool tracking = false)
    {
        if (!tracking)
        {
            context.Orders.AsNoTracking();
        }
        var result = await context.Orders
        .Include(x => x.OrderItems).ThenInclude(x => x.OrdersDiscount)
        .Include(x => x.OrderItems).ThenInclude(x => x.Part).ThenInclude(x => x.PartImages)
        .Where(predicate).ToListAsync();
        return result;
    }
}