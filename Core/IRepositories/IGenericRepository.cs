
using System.Linq.Expressions;

namespace StackApi.Core.IRepositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> All();
    Task<T> GetByID(Guid ID);
    Task<bool> Add(T Entity);
    Task<bool> Delete(Guid ID);
    Task<bool> Update(T Entity);
    Task<IEnumerable<T>> getByCondition(Expression<Func<T,bool>> predicate);
    Task<T> getFirstByCondition(Expression<Func<T,bool>> predicate);
}