using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Infrastructure;
using WebAPI.Intrastructure;

namespace WebAPI.Accounts.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Result>
    {
        public string CurrentPassword { get; set; } = default!;

        public string NewPassword { get; set; } = default!;

        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
        {
            private readonly IIdentity _userService;
            private readonly ICurrentUser _currentUser;

            public ChangePasswordCommandHandler(IIdentity userService, ICurrentUser currentUser)
            {
                _userService = userService;
                _currentUser = currentUser;
            }

            public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
                => await _userService.ChangePassword(new ChangePasswordInputModel(_currentUser.UserId, request.CurrentPassword, request.NewPassword));
        }
    }
}
