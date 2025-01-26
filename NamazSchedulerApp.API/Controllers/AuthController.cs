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
        private readonly GetHashPassword _getHashPassword;

        public AuthController(AppDbContext context, ILogger<AuthController> logger, GetHashPassword getHashPassword)
        {
            _context = context;
            _logger = logger;
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
        public IActionResult Login([FromBody] LoginRequest loginRequest)
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

                _logger.LogInformation($"User logged in successfully: {loginRequest.Email}");
                return Ok(new { Message = "Login successful." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user login.");
                return StatusCode(500, "An unexpected error occurred during login.");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == forgotPasswordRequest.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Forgot password failed: User not found for email {forgotPasswordRequest.Email}");
                    return NotFound("User with the specified email does not exist.");
                }

                // Generate a random numeric token (6 digits in this case)
                var random = new Random();
                var resetToken = string.Concat(Enumerable.Range(0, 6).Select(_ => random.Next(0, 10).ToString()));

                // Log the reset token (you would typically send this via email)
                _logger.LogInformation($"Generated reset token for user {user.Email}: {resetToken}");

                // TODO: Implement email sending logic here
                // Example: SendEmail(user.Email, "Password Reset", $"Your reset token is: {resetToken}");

                // Save the reset token to the database
                user.ResetToken = resetToken;
                user.TokenExpiry = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok("A password reset token has been sent to your email address.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the forgot password process.");
                return StatusCode(500, "An unexpected error occurred while processing the forgot password request.");
            }
        }

    }
}
