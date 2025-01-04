using PO_Task.Application.Abstractions.Messaging;

namespace PO_Task.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand<Guid>;
