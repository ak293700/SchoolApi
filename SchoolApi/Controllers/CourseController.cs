using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO;
using SchoolApi.Models;
using SchoolApi.Services;

namespace SchoolApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<IEnumerable<LiteCourseDTO>> GetAll()
        {
            return (await _courseService.GetAll()).Select(c => new LiteCourseDTO(c));
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult<LiteCourseDTO>> GetOne(int id)
        {
            Course? course = await _courseService.GetOne(id);
            if (course == null)
               return NotFound();
            return new LiteCourseDTO(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")] // Authorize Admin || Teacher
        public async Task<ActionResult<LiteCourseDTO>> CreateOne(string name)
        { 
            Course result = await _courseService.CreateOne(name);
            return CreatedAtAction("GetOne", new {id = result.Id}, new LiteCourseDTO(result));
        }

        /// <returns>Empty response. 400 if the course hasn't been found, 200 else</returns>
        [HttpDelete("{id}"), Authorize(Roles = "Admin")] // Authorize Admin 
        public async Task<IActionResult> DeleteOne(int id)
        {
            bool deleted = await _courseService.DeleteOne(id);
            return deleted ? Ok() : NotFound();
        }
    }
}
