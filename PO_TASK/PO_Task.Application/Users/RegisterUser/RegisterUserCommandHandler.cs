using PO_Task.Application.Abstractions.Messaging;
using PO_Task.Domain.BuildingBlocks;
using PO_Task.Domain.Users;

namespace PO_Task.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = User.CreateInstance(
            new FirstName(request.FirstName),
            new LastName(request.LastName),
            new Email(request.Email));

        await _userRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id.Value;
    }
}
