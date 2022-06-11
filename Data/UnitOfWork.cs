using StackApi.Core.IConfiguration;
using StackApi.Core.IRepositories;
using StackApi.Core.Repositories;

namespace StackApi.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly PartDbContext context;
    private readonly ILogger logger;
    public IUserRepository Users { get; private set; }
    public IUserDetailsRepository userDetailsRepository { get; private set; }
    public IPartRepository partRepository { get; private set; }
    public IPartImageRepository partImageRepository { get; private set; }
    public ISearchViewHistoryRepository searchViewHistoryRepository { get; private set; }
    public IDiscountRepository discountRepository { get; private set; }
    public ICartItemsRepository cartItemsRepository { get; private set; }

    public UnitOfWork(PartDbContext _context, ILoggerFactory loggerFactory)
    {
        context = _context;
        logger = loggerFactory.CreateLogger("logs");
        Users = new UserRepository(context, logger);
        userDetailsRepository = new UserDetailsRepository(context, logger);
        partRepository = new PartRepository(context, logger);
        partImageRepository = new PartImageRepository(context, logger);
        searchViewHistoryRepository = new SearchViewHistoryRepository(context, logger);
        discountRepository = new DiscountRepository(context, logger);
        cartItemsRepository = new CartItemsRepository(context, logger);
    }

    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}