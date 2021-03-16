using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Common;
using WebAPI.Infrastructure;

namespace WebAPI.Accounts.GetUser
{
    public class GetUserQuery : IRequest<Result<UserOutputModel>>
    {
        public GetUserQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }

        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserOutputModel>>
        {
            private readonly IIdentity _userService;

            public GetUserQueryHandler(IIdentity userService)
            {
                _userService = userService;
            }

            public async Task<Result<UserOutputModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var result = await _userService.GetUsersByIdAsync(request.Id);

                if (!result.Succeeded)
                {
                    return result.Errors;
                }

                return result.Data;
            }
        }
    }
}
