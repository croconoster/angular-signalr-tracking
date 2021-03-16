using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Infrastructure;

namespace WebAPI.Accounts.LoginUser
{
    public class LoginUserCommand : UserInputModel, IRequest<Result<LoginOutputModel>>
    {
        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginOutputModel>>
        {
            private readonly IIdentity _identityService;

            public LoginUserCommandHandler(IIdentity identityService)
            {
                _identityService = identityService;
            }

            public async Task<Result<LoginOutputModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                var result = await _identityService.Login(request);
                if (!result.Succeeded)
                {
                    return result.Errors;
                }

                return new LoginOutputModel(result.Data.Token, result.Data.UserId, result.Data.FirstName, result.Data.LastName, result.Data.Email, result.Data.Role);
            }
        }
    }
}
