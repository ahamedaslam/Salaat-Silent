using AuthenticationServiNamazSchedulerApp.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NamazSchedulerApp.API.Context;
using NamazSchedulerApp.API.DTOs;
using NamazSchedulerApp.API.Models;
using NamazSchedulerApp.API.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NamazSchedulerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly GenerateToken _generateToken;
        private readonly GetHashPassword _getHashPassword;

        public AuthController(AppDbContext context, ILogger<AuthController> logger, GenerateToken generateToken, GetHashPassword getHashPassword)
        {
            _context = context;
            _logger = logger;
            _generateToken = generateToken;
            _getHashPassword = getHashPassword;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    _logger.LogWarning($"Attempted registration with an already registered email: {user.Email}");
                    return BadRequest("Email already registered.");
                }

                // Use GetHashPassword to hash the password
                user.PasswordHash = _getHashPassword.GetHashValue(user.PasswordHash);

                user.UserId = Guid.NewGuid();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User registered successfully: {user.Email}");
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration.");
                return StatusCode(500, "An unexpected error occurred during registration.");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginRequest)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == loginRequest.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Login failed: User not found for email {loginRequest.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                // Hash the input password and compare
                var computedHash = _getHashPassword.GetHashValue(loginRequest.Passwod);
                if (user.PasswordHash != computedHash)
                {
                    _logger.LogWarning($"Login failed: Invalid password for email {loginRequest.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                // Generate JWT token
                var token = _generateToken.CreateJwtToken(user.UserId);
                _logger.LogWarning($"The userID is {user.UserId}");

                _logger.LogInformation($"User logged in successfully: {loginRequest.Email}");
                return Ok(new { Token = token, Message = "Login successful." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login.");
                return StatusCode(500, "An unexpected error occurred during login.");
            }
        }
    }
}
