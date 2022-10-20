using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO.CourseDTO;
using SchoolApi.Models.CourseModels;
using SchoolApi.Services.CourseServices;

namespace SchoolApi.Controllers.CourseControllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IEnumerable<LiteCourseDTO>> GetAll()
        {
            return await _courseService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LiteCourseDTO>> GetOne(int id)
        {
            LiteCourseDTO? course = await _courseService.GetOne(id);
            if (course == null)
                return NotFound();
            return course;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")] // Authorize Admin || Teacher
        public async Task<ActionResult<LiteCourseDTO>> CreateOne(string name)
        {
            Course result = await _courseService.CreateOne(name);
            return CreatedAtAction("GetOne", new { id = result.Id }, new LiteCourseDTO(result));
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