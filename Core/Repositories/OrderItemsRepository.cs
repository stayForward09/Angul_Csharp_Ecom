using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class OrderItemsRepository : GenericRepository<OrderItems>, IOrderItemsRepository
{
    public OrderItemsRepository(PartDbContext _context, ILogger _logger) : base(_context, _logger)
    {
    }

    public async Task AddRange(List<OrderItems> entites)
    {
        await context.AddRangeAsync(entites);
    }
}