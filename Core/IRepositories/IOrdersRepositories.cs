using System.Linq.Expressions;
using StackApi.Models;

namespace StackApi.Core.IRepositories;

public interface IOrdersRepositories : IGenericRepository<Orders>
{
    Task<List<Orders>> getOrderbyCondition(Expression<Func<Orders, bool>> predicate, bool tracking = false);
}