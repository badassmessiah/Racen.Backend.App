using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Racen.Backend.App.Data;
using Racen.Backend.App.Models;
using Racen.Backend.App.Models.User;
using Racen.Backend.App.DTOs;
using Racen.Backend.App.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Racen.Backend.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountService accountService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var result = await _accountService.RegisterUserAsync(model);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = result.Errors.FirstOrDefault()?.Description });

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var result = await _accountService.LoginAsync(model);
            if (!result.Succeeded)
                return Unauthorized(new { Status = "Error", Message = result.Errors.FirstOrDefault()?.Description });

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) // Fix: Check if user is null
                return Unauthorized(new { Status = "Error", Message = "Invalid username or password." });

            var (token, refreshToken) = await _accountService.GenerateTokensAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                refreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest model)
        {
            _logger.LogInformation("Received refresh token request");

            var (newToken, newRefreshToken) = await _accountService.RefreshTokenAsync(model);

            if (newToken == null || newRefreshToken == null)
            {
                _logger.LogWarning("Invalid token or refresh token");
                return Unauthorized(new { message = "Invalid token or refresh token" });
            }

            _logger.LogInformation("Refresh token process completed successfully");
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(newToken),
                expiration = newToken.ValidTo,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));

                if (result.Succeeded)
                {
                    return Ok(new { message = "Role created successfully" });
                }
                return BadRequest(new { message = "Role creation failed" });
            }
            return BadRequest(new { message = "Role already exists" });
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest(new { message = "User not found" });
            }
            var result = await _userManager.AddToRoleAsync(user, model.Role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned successfully" });
            }
            return BadRequest(new { message = "Role assignment failed" });
        }
    }
}