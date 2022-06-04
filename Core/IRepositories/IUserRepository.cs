using System.Linq.Expressions;
using StackApi.Dtos;
using StackApi.Models;

namespace StackApi.Core.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> CheckEmailExists(string mailid);
    Task<User> GetUserbyCondition(Expression<Func<User, bool>> condition,bool disableTracking = true);
}