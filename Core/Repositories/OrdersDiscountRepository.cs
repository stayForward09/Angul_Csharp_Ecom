using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class OrdersDiscountRepository : GenericRepository<OrdersDiscount>, IOrdersDiscountRepository
{
    public OrdersDiscountRepository(PartDbContext _context, ILogger _logger) : base(_context, _logger)
    {
    }
}