using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Infrastructure;

namespace WebAPI.Accounts.CreateUser
{
    public class CreateUserCommand : CreateUserInputModel, IRequest<Result>
    {
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
        {
            private readonly IIdentity _userService;

            public CreateUserCommandHandler(IIdentity userService) 
                => _userService = userService;

            public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken) 
                => await _userService.Create(request);
        }
    }
}
