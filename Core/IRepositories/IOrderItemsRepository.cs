using StackApi.Models;

namespace StackApi.Core.IRepositories;

public interface IOrderItemsRepository : IGenericRepository<OrderItems>
{
    Task AddRange(List<OrderItems> entites);
}