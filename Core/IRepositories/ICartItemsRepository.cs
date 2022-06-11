using StackApi.Models;

namespace StackApi.Core.IRepositories;

public interface ICartItemsRepository : IGenericRepository<CartItems>
{
   Task<bool> RemoveCartItem(Guid Id);
}