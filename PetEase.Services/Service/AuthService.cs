using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetEase.Core.DTOs;
using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PetEase.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt,
            IWebHostEnvironment env,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        // Register a new user
        public async Task<AuthDTOs> RegisterAsync(RegisterDTOs registerDTO)
        {
            var username = registerDTO.Email.Split('@')[0];

            if (await _userManager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return new AuthDTOs { Message = "Email is already registered!", IsAuthenticated = false };
            }

            if (await _userManager.FindByNameAsync(username) != null)
            {
                return new AuthDTOs { Message = "Username is already taken!", IsAuthenticated = false };
            }

            var user = new ApplicationUser
            {
                UserName = username,
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return new AuthDTOs
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    IsAuthenticated = false
                };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthDTOs
            {
                Message = "User registered successfully",
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                ExpiresOn = token.ValidTo,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }

        // Login an existing user
        public async Task<AuthDTOs> LoginAsync(LoginDTOs loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password) || !user.EmailConfirmed)
            {
                return new AuthDTOs { Message = "Email or Password is incorrect!", IsAuthenticated = false };
            }

            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);

            if (activeRefreshToken == null)
            {
                var newRefreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
                activeRefreshToken = newRefreshToken;
            }

            return new AuthDTOs
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                ExpiresOn = token.ValidTo,
                RefreshToken = activeRefreshToken.Token,
                RefreshTokenExpiration = activeRefreshToken.ExpiresOn
            };
        }


        // Refresh Token
        public async Task<AuthDTOs> RefreshTokenAsync(string token)
        {
            AuthDTOs authDTOs = new AuthDTOs();
            var user = _userManager.Users.SingleOrDefault(X => X.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                authDTOs.Message = "Invalid Token";
                return authDTOs;
            }
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
            {
                authDTOs.Message = "Inactive Token";
                return authDTOs;
            }
            refreshToken.RevokedOn = DateTime.Now;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtSecurityToken = await CreateJwtToken(user);
            authDTOs.IsAuthenticated = true;
            authDTOs.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDTOs.Email = user.Email;
            authDTOs.UserName = user.UserName;
            authDTOs.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            authDTOs.RefreshToken = newRefreshToken.Token;
            authDTOs.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            return authDTOs;
        }
        // Revoke Token
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = _userManager.Users.SingleOrDefault(X => X.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return true;
        }

        // Generate a JWT token
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim("roles", role));

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim("name", user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: creds
            );
        }

        // Generate a secure refresh token
        private RefreshToken GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var token = Convert.ToBase64String(randomBytes)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty)
                .Replace("=", string.Empty);

            return new RefreshToken
            {
                Token = token,
                CreateOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(3)
            };
        }
    }
}
