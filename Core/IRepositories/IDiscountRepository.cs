using System.Linq.Expressions;
using StackApi.Models;

namespace StackApi.Core.IRepositories;

public interface IDiscountRepository : IGenericRepository<Discount>
{
    Task<decimal> getDiscountPrice(Expression<Func<Discount, bool>> predicate);
}