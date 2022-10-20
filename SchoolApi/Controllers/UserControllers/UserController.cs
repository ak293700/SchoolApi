using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO;
using SchoolApi.Models.UserModels;
using SchoolApi.Services.UserServices;

namespace SchoolApi.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<LiteUserDTO>> GetAll()
        {
            return (await _userService.GetAll()).Select(c => new LiteUserDTO(c));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LiteUserDTO>> GetOne(int id)
        {
            User? user = await _userService.GetOne(id);
            if (user == null)
                return NotFound();
            return new LiteUserDTO(user);
        }

        /// <returns>Empty response. 400 if the user hasn't been found, 200 else</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            bool deleted = await _userService.DeleteOne(id);
            return deleted ? Ok() : NotFound();
        }
    }
}