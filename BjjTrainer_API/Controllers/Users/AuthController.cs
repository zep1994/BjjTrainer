using BjjTrainer_API.Models.Users;
using BjjTrainer_API.Services_API.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BjjTrainer_API.Controllers.Users
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserService _userService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    JwtTokenService jwtTokenService,
                                    UserService userService,
                                    ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _logger = logger;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                SchoolId = model.SchoolId > 0 ? model.SchoolId : null,
                Role = model.Role
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Errors = errorMessages });
            }

            // Enroll the user in school events if a SchoolId is provided
            if (model.SchoolId != null && model.SchoolId > 0)
            {
                await _userService.EnrollUserInSchoolEvents(user.Id, model.SchoolId.Value);
            }

            var token = _jwtTokenService.GenerateToken(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName ?? string.Empty), // Fix: Ensure non-null value
                new(ClaimTypes.Role, user.Role.ToString()),
                new("SchoolId", user.SchoolId?.ToString() ?? string.Empty)
            });

            return Ok(new { Token = token });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid login attempt");
            }

            // Generate JWT token with the IsCoach claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty), // Fix: Ensure non-null value
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var token = _jwtTokenService.GenerateToken(claims);
            return Ok(new { Token = token });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshToken refreshTokenDto)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenDto.Token))
            {
                return BadRequest("Refresh token is required.");
            }

            var newTokens = await _jwtTokenService.RefreshTokenAsync(refreshTokenDto.Token);
            return newTokens == null ? Unauthorized("Invalid or expired refresh token.") : Ok(newTokens);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Here, implement any logic for token invalidation if necessary
            return Ok(new { Message = "Logged out successfully" });
        }
    }

    public class RegisterModel
    {
        public required string Username { get; set; } 
        public required string Email { get; set; }    
        public required string Password { get; set; } 
        public int? SchoolId { get; set; }
        public UserRole Role { get; set; } = UserRole.Student;
    }

    public class LoginModel
    {
        public required string Username { get; set; } 
        public required string Password { get; set; } 
    }
}
