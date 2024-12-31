using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Domain.Users;

public sealed record UserProfile : ValueObject
{
    private UserProfile(FirstName firstName, LastName lastName, Email email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private UserProfile()
    {
    }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }


    public static UserProfile Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new UserProfile(firstName, lastName, email);
        return user;
    }
}
