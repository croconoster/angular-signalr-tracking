using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyTransfer.Application.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data.Models;

namespace WebAPI.Infrastructure
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IList<string> roles);
    }

    public class JwtTokenGeneratorService : IJwtTokenGenerator
    {
        private readonly ApplicationSettings _applicationSettings;

        public JwtTokenGeneratorService(IOptions<ApplicationSettings> applicationSettings)
            => this._applicationSettings = applicationSettings.Value;

        public string GenerateToken(User user, IList<string> roles)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_applicationSettings.JWT.Secret);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Email ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);            

            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.UtcNow,
                Issuer = _applicationSettings.JWT.Issuer,
                Audience = _applicationSettings.JWT.Audience
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
