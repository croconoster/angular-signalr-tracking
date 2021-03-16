using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Infrastructure;
using WebAPI.Intrastructure;

namespace WebAPI.Accounts.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<Result<IEnumerable<UserOutputModel>>>
    {
        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserOutputModel>>>
        {
            private readonly IIdentity _userService;
            private readonly ICurrentUser _currentUser;

            public GetAllUsersQueryHandler(IIdentity userService, ICurrentUser currentUser)
            {
                this._userService = userService;
                _currentUser = currentUser;
            }

            public async Task<Result<IEnumerable<UserOutputModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var result = await _userService.GetAllUsersAsync(_currentUser.UserId);
                if (!result.Succeeded)
                {
                    return result.Errors;
                }

                return result.Data.ToList();
            }
        }
    }
}
