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

    public async Task<decimal> getDiscountPrice(Expression<Func<Discount, bool>> predicate)
    {
        var data = await context.Discount.FirstOrDefaultAsync(predicate);
        return data?.Amount ?? 0;
    }
}