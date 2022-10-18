using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO;
using SchoolApi.Models;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<IEnumerable<LiteUserDTO>> GetAll()
        {
            return (await _userService.GetAll()).Select(c => new LiteUserDTO(c));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LiteUserDTO>> GetOne(int id)
        {
            User? user = await _userService.GetOne(id);
            if (user == null)
                return NotFound();
            return new LiteUserDTO(user);
        }
        
        [HttpPost]
        public async Task<ActionResult<LiteUserDTO>> CreateOne(string username, string password)
        { 
            User result = await _userService.CreateOne(username, password);
            return CreatedAtAction("GetOne", new {id = result.Id}, new LiteUserDTO(result));
        }

        /// <returns>Empty response. 400 if the user hasn't been found, 200 else</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            bool deleted = await _userService.DeleteOne(id);
            return deleted ? Ok() : NotFound();
        }
    }
}