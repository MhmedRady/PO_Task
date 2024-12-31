using PO_Task.Domain.Users;

namespace PO_Task.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
