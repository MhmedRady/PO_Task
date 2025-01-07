namespace PO_Task.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(User purchaseOrder);
}
