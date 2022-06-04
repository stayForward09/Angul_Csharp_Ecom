using StackApi.Models;
using StackApi.Core.IRepositories;
using StackApi.Data;

namespace StackApi.Core.Repositories;

public class SearchViewHistoryRepository : GenericRepository<SearchViewHistory>, ISearchViewHistoryRepository
{
    public SearchViewHistoryRepository(PartDbContext _context, ILogger _logger) : base(_context, _logger)
    {
    }
}