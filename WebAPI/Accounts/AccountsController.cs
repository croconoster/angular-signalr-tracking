using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Accounts.ChangePassword;
using WebAPI.Accounts.CreateUser;
using WebAPI.Accounts.GetAllUsers;
using WebAPI.Accounts.GetUser;
using WebAPI.Accounts.LoginUser;
using WebAPI.Accounts.UpdateUser;
using WebAPI.Common;

namespace WebAPI.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMediator _mediator;

        public AccountsController(ILogger<AccountsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginOutputModel>> Login(LoginUserCommand command)
            => await _mediator.Send(command).ToActionResultAsync();

        [HttpPut]
        [Route(nameof(ChangePassword))]
        [Authorize]
        public async Task<ActionResult<Result>> ChangePassword(ChangePasswordCommand command)
            => await _mediator.Send(command).ToActionResultAsync();

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult> Create(CreateUserCommand command)
            => await _mediator.Send(command).ToActionResultAsync();

        [HttpPut]
        [Authorize]
        [Route(nameof(Update))]
        public async Task<ActionResult> Update(UpdateUserCommand command)
            => await _mediator.Send(command).ToActionResultAsync();

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserOutputModel>>> GetAll()
            => await _mediator.Send(new GetAllUsersQuery()).ToActionResultAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<UserOutputModel>> GetUser(string id)
            => await _mediator.Send(new GetUserQuery(id)).ToActionResultAsync();

    }
}
