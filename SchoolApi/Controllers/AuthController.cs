using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolApi.DTO;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<bool>> Register(RegisterUserDTO user)
        {
            bool result = await _authService.Register(user);
            if (result == false)
                return BadRequest("Email already registered");
            
            return Ok(result);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(RegisterUserDTO user)
        {
            string? result = await _authService.Login(user);
            if (string.IsNullOrEmpty(result))
                return BadRequest("Wrong email or password");
            
            return Ok(result);
        }
        
        /*
        Use to generate password's data for database 
        [HttpGet]
        public async Task<ActionResult<object>> Get(string password)
        {
            _authService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            // string hash = System.Text.Encoding.UTF8.GetString(passwordHash); 
            // string salt = System.Text.Encoding.UTF8.GetString(passwordSalt);
            string hash = Convert.ToBase64String(passwordHash);
            string salt = Convert.ToBase64String(passwordSalt);
            return Ok(new 
            {
                hash,
                salt
            });
        }*/
    }
}