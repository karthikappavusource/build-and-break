using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TokenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public TokenController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpGet("{mail}")]
        public async Task<ActionResult<IdentityUser>> GetUser(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            return Ok(user);
        }
        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel loginModel)
        {
            var condition = await IsValidUser(loginModel.Email, loginModel.Password);
            if (condition)
            {
                var token = GenerateJwtToken(loginModel.Email);
                return Ok(new { token });
            }
            return Unauthorized();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var username = model.Email.Split('@')[0];
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if ( userExist== null)
            {
                var user = new IdentityUser { UserName = username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                return Ok(result.Succeeded);
            }
            return BadRequest();
        }
        private async Task<bool> IsValidUser(string email, string password)
        {
            // Validate the user credentials here (e.g., against a database)
            // This is a simplified example with hardcoded user validation
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }

        private string GenerateJwtToken(string email)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Email, email),
                new Claim("CustomClaim1", "CustomValue1"),
                new Claim("CustomClaim2", "CustomValue2"),
                new Claim("CustomClaim3", "CustomValue3"),
                new Claim("CustomClaim4", "CustomValue4"),
                new Claim("CustomClaim5", "CustomValue5")
                }),
                Expires = DateTime.UtcNow.AddSeconds(600),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
