using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StackApi.Core.IRepositories;
using StackApi.Core.Repositories;
using StackApi.Data;
using StackApi.Dtos;
using StackApi.Models;

public class CartItemsRepository : GenericRepository<CartItems>, ICartItemsRepository
{
    public CartItemsRepository(PartDbContext context, ILogger logger) : base(context, logger)
    {

    }

    public async Task<bool> RemoveCartItem(Guid Id)
    {
        var data = await GetByID(Id);
        context.Remove(data);
        return true;
    }
}