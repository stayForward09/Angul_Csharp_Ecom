using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
{
    public DiscountRepository(PartDbContext dbContext, ILogger logger) : base(dbContext, logger)
    {

    }

    public async Task<Discount> fetchDiscountbyCondition(Expression<Func<Discount, bool>> predicate, bool tracking = false)
    {
        if (!tracking)
        {
            context.Discount.AsNoTracking();
        }
        var data = await context.Discount.FirstOrDefaultAsync(predicate);
        return data;
    }

    public async Task<IEnumerable<Discount>> fetchDiscountsbyCondition(Expression<Func<Discount, bool>> predicate, bool tracking = false)
    {
        if (!tracking)
        {
            context.Discount.AsNoTracking();
        }
        var data = await context.Discount.Where(predicate).ToListAsync();
        return data;
    }

    public async Task<decimal> getDiscountPrice(Expression<Func<Discount, bool>> predicate)
    {
        var data = await context.Discount.FirstOrDefaultAsync(predicate);
        return data?.Amount ?? 0;
    }
}