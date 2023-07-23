using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ManagementProject.Application.Contracts.Identity;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Models.Authentication;
using ManagementProject.Identity.Entity;
using ManagementProject.Identity.JwtModel;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.Results;

namespace ManagementProject.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new NotFoundException(nameof(username), username);
            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                throw new ValidationException($"Credentials for '{username} aren't valid'.");
            }

            /********TODO For email validation in a real application ********/
            /*
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, lockoutOnFailure: false);
                    if (!result.Succeeded)
                    {
                        throw new ValidationException($"Credentials for '{username} aren't valid'.");
                    }
            */

            JwtSecurityToken? jwtSecurityToken = await GenerateToken(user);

            AuthenticationResponse response = new()
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            return response;
        }

        private async Task<JwtSecurityToken?> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            }
            .Union(userClaims);

            if(_jwtSettings.Key != null)
            {
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signingCredentials);
                return jwtSecurityToken;
            }
            return null;
        }
    }
}
