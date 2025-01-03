using Microsoft.AspNetCore.Identity;
using Racen.Backend.App.Data;
using Racen.Backend.App.Models;
using Racen.Backend.App.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Racen.Backend.App.DTOs;
using Microsoft.EntityFrameworkCore;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<AccountService> _logger;
        

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDbContext context, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _logger = logger;

        }

        public async Task<IdentityResult> RegisterUserAsync(Register model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return IdentityResult.Failed(new IdentityError { Description = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return result;

            // Assign "User" role to the newly registered user
            if (!await _roleManager.RoleExistsAsync("user"))
                await _roleManager.CreateAsync(new IdentityRole("user"));

            await _userManager.AddToRoleAsync(user, "user");

            
            await CreateInitialMotorcycleForUserAsync(user.Id);

            return IdentityResult.Success;
        }

        private async Task CreateInitialMotorcycleForUserAsync(string userId)
        {
            var owner = await _userManager.FindByIdAsync(userId)
                ?? throw new InvalidOperationException("User not found");

            var motorcycle = new Motorcycle
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Initial Motorcycle",
                Rarity = Rarity.Basic,
                OwnerId = userId,
                Owner = owner
            };
            
            DefaultProperties.SetDefaultProperties(motorcycle);

            await _context.Motorcycles.AddAsync(motorcycle);
            _logger.LogInformation("Initial motorcycle created for user ID: {UserId}", userId);
            await _context.SaveChangesAsync();

            
        }


        public async Task<JwtSecurityToken> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Generating JWT token for user ID: {UserId}", user.Id);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            foreach (var claim in authClaims)
            {
                _logger.LogInformation("Adding claim: {Type} = {Value}", claim.Type, claim.Value);
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JWT:ExpireMinutes"] ?? "60")),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? string.Empty)),
                SecurityAlgorithms.HmacSha256)
            );

            _logger.LogInformation("JWT token generated successfully for user ID: {UserId}", user.Id);

            return token;
        }

        public string GenerateRefreshToken()
        {
            _logger.LogInformation("Generating a new refresh token");

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                _logger.LogInformation("New refresh token generated: {RefreshToken}", refreshToken);
                return refreshToken;
            }
        }

        public async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            _logger.LogInformation("Saving refresh token for user {UserId}", userId);

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                ExpiryDate = DateTime.Now.AddDays(7) // Set refresh token expiry
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Refresh token saved successfully for user {UserId}", userId);
        }

        public async Task<IdentityResult> LoginAsync(Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = "Invalid login attempt." });
        }

        public async Task<(JwtSecurityToken, string)> GenerateTokensAsync(ApplicationUser user)
        {
            var token = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(user.Id, refreshToken);
            return (token, refreshToken);
        }

        public async Task<(JwtSecurityToken, string)> RefreshTokenAsync(TokenRequest model)
        {
            _logger.LogInformation("Starting refresh token process");

            var principal = GetPrincipalFromExpiredToken(model.Token);
            if (principal == null)
            {
                _logger.LogWarning("Invalid principal from expired token");
                return (null!, null!); // Fix: Use null-forgiving operator
            }

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogWarning("User ID not found in token claims");
                return (null!, null!); // Fix: Use null-forgiving operator
            }

            _logger.LogInformation($"Attempting to find refresh token for user ID: {userId}");

            var storedRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == model.RefreshToken && rt.UserId == userId && !rt.IsRevoked);

            if (storedRefreshToken == null)
            {
                _logger.LogWarning("Stored refresh token not found or is revoked");
                _logger.LogInformation("Refresh token: {RefreshToken}, User ID: {UserId}", model.RefreshToken, userId);
                var allTokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
                _logger.LogInformation("All tokens for user ID {UserId}: {Tokens}", userId, string.Join(", ", allTokens.Select(t => t.Token)));
                return (null!, null!); // Fix: Use null-forgiving operator
            }

            if (storedRefreshToken.ExpiryDate <= DateTime.Now)
            {
                _logger.LogWarning("Stored refresh token is expired");
                return (null!, null!); // Fix: Use null-forgiving operator
            }

            // Revoke the old refresh token
            storedRefreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Old refresh token revoked");

            // Generate new tokens
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found");
                return (null!, null!); // Fix: Use null-forgiving operator
            }

            var (newToken, newRefreshToken) = await GenerateTokensAsync(user);
            _logger.LogInformation("New tokens generated successfully");
            return (newToken, newRefreshToken);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? string.Empty)),
                ValidateLifetime = false // Here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            _logger.LogInformation("Extracted claims from expired token:");
            foreach (var claim in principal.Claims)
            {
                _logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
            }

            return principal;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id) ?? throw new InvalidOperationException("User not found");
        }

        public async Task SetPlayerWinAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException("User not found");
            user.Wins++;
            await _userManager.UpdateAsync(user);
        }

        public async Task SetPlayerLossAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException("User not found");
            user.Losses++;
            await _userManager.UpdateAsync(user);
        }

        public async Task SetTotalMatchesPlayedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException("User not found");
            user.MatchesPlayed++;
            await _userManager.UpdateAsync(user);
        }

        public async Task SetPlayerMoneyAsync(string userId, decimal amount)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException("User not found");
            user.Money += amount;
            await _userManager.UpdateAsync(user);
        }


        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            _logger.LogInformation("Password reset token generated for user ID: {UserId}", user.Id);
            return token;
        }

        public async Task SendPasswordResetTokenAsync(string email)
        {
            var token = await GeneratePasswordResetTokenAsync(email);
            // Here you would send the token to the user's email.
            // For simplicity, we are just logging it.
            _logger.LogInformation("Password reset token for email {Email}: {Token}", email, token);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successfully for user ID: {UserId}", user.Id);
            }
            else
            {
                _logger.LogWarning("Password reset failed for user ID: {UserId}", user.Id);
            }

            return result;
        }

    }
}