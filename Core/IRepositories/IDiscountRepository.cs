using System.Linq.Expressions;
using StackApi.Models;

namespace StackApi.Core.IRepositories;

public interface IDiscountRepository : IGenericRepository<Discount>
{
    Task<decimal> getDiscountPrice(Expression<Func<Discount, bool>> predicate);
    Task<Discount> fetchDiscountbyCondition(Expression<Func<Discount, bool>> predicate, bool tracking = false);
    Task<IEnumerable<Discount>> fetchDiscountsbyCondition(Expression<Func<Discount, bool>> predicate, bool tracking = false);
}