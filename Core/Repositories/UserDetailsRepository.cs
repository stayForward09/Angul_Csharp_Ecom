using StackApi.Core.IRepositories;
using StackApi.Data;
using StackApi.Models;

namespace StackApi.Core.Repositories;

public class UserDetailsRepository : GenericRepository<UserDetails>, IUserDetailsRepository
{
    public UserDetailsRepository(PartDbContext context, ILogger logger) : base(context, logger)
    {

    }
}