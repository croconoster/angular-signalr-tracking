using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Infrastructure;
using WebAPI.Intrastructure;

namespace WebAPI.Accounts.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
        {
            private readonly ICurrentUser _currentUser;
            private readonly IIdentity _userService;

            public UpdateUserCommandHandler(IIdentity userService, ICurrentUser currentUser)
            {
                _userService = userService;
                _currentUser = currentUser;
            }

            public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken) 
                => await _userService.Update(new UpdateUserInputModel(
                                                                        _currentUser.UserId,
                                                                        request.FirstName,
                                                                        request.LastName,
                                                                        request.Email));
        }
    }
}
