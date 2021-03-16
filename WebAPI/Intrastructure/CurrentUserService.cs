using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Data.Models;
using WebAPI.Infrastructure;

namespace WebAPI.Intrastructure
{
    public interface ICurrentUser
    {
        string UserId { get; }

        string FullName { get; }
    }


    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user != null)
            {
                this.UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                this.FullName = $"{user.FindFirstValue(ClaimTypes.GivenName)} {user.FindFirstValue(ClaimTypes.Surname)}";
            }
        }

        public string UserId { get; }

        public string FullName { get; }
    }
}
