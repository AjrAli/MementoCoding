using ManagementProject.Application.Contracts.Identity;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Models.Account;
using ManagementProject.Application.Models.Account.Command;
using ManagementProject.Identity.Entity;
using ManagementProject.Identity.JwtModel;
using ManagementProject.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ManagementProject.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new JwtSecurityTokenHandler();

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AccountResponse> AuthenticateAsync(string username, string password)
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
            var userRoles = await _userManager.GetRolesAsync(user); // Get the roles of the user
            JwtSecurityToken? jwtSecurityToken = await GenerateToken(user, userRoles);

            return new AccountResponse
            {
                Id = user.Id,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                Role = userRoles.FirstOrDefault()
            };
        }

        public async Task<AccountResponse> CreateSimpleUserAsync(CreateSimpleUserCommand request)
        {
            var newUser = new ApplicationUser()
            {
                FirstName = request.Account.FirstName,
                LastName = request.Account.LastName,
                Email = request.Account.Email,
                UserName = request.Account.Username,
                EmailConfirmed = true //For test app only
            };
            var resultUser = await _userManager.CreateAsync(newUser, request.Account.Password);
            if (!resultUser.Succeeded)
                throw new BadRequestException($"Failed to create user {newUser?.UserName}", resultUser.Errors.ToList().Select(x => x.Description).ToList());
            var resultRole = await _userManager.AddToRoleAsync(newUser, RoleNames.User);
            if (!resultRole.Succeeded)
                throw new BadRequestException($"Failed to create user {newUser?.UserName}", resultRole.Errors.ToList().Select(x => x.Description).ToList());

            var user = await _userManager.FindByNameAsync(newUser.UserName);
            var userRoles = await _userManager.GetRolesAsync(user); // Get the roles of the user
            JwtSecurityToken? jwtSecurityToken = await GenerateToken(user, userRoles);
            return new AccountResponse
            {
                Id = user.Id,
                Token = _jwtTokenHandler.WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                Role = userRoles.FirstOrDefault()
            };
        }

        private async Task<JwtSecurityToken?> GenerateToken(ApplicationUser user, IList<string> userRoles)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            }
            .Union(userClaims)
            .Union(userRoles.Select(role => new Claim(ClaimTypes.Role, role))); // Add the roles as claims

            if (_jwtSettings.Key != null)
            {
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                // Adjust token expiration as needed (e.g., 1 hour)
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10), // Change to your desired expiration time
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }

            return null;
        }
    }
}
