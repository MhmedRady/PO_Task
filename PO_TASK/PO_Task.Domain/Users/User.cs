
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Users.Events;

namespace PO_Task.Domain.Users;

public sealed class User : Entity<UserId>
{
    private User(UserId id)
    : base(id)
    {
    }

    private User()
    {
    }
    public UserProfile Profile { get; private set; }

    public static User CreateInstance(
        FirstName firstName,
        LastName lastName,
        Email email)
    {
        var user = new User(UserId.CreateUnique());


        user.RaiseDomainEvent(new UserCreatedDomainEvent { Id = user.Id.Value });

        user.Profile = UserProfile.Create(firstName, lastName, email);

        return user;
    }
}
