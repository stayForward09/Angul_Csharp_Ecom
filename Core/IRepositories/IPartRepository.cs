using StackApi.Models;

namespace StackApi.Core.IRepositories;

public interface IPartRepository : IGenericRepository<Part>
{
    Task<object> SearchbyText(string searchText);
}