using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO;
using SchoolApi.Models;
using SchoolApi.Services;

namespace SchoolApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
[Authorize]
public class EnrollmentController : ControllerBase
{
    private readonly EnrollmentService _enrollmentService;

    public EnrollmentController(EnrollmentService enrollmentService, IHttpContextAccessor httpContextAccessor)
    {
        _enrollmentService = enrollmentService;
    }
    
    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<LiteEnrollmentDTO>> GetAll()
    {
        return (await _enrollmentService.GetAll()).Select(e => new LiteEnrollmentDTO(e));
    }
    
    [HttpGet("{id}"), AllowAnonymous]
    public async Task<ActionResult<LiteEnrollmentDTO>> GetOne(int id)
    {
        Enrollment? enrollment = await _enrollmentService.GetOne(id);
        if (enrollment == null)
            return NotFound();
        
        return new LiteEnrollmentDTO(enrollment);
    }
    
    
    [HttpPost]
    public async Task<ActionResult<LiteEnrollmentDTO>> CreateOne(int courseId, int studentId)
    {
        Enrollment? enrollment = await _enrollmentService.CreateOne(courseId, studentId);
        if (enrollment == null)
            return NotFound();

        return CreatedAtAction("GetOne", new { id = enrollment.Id }, new LiteEnrollmentDTO(enrollment));
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteOne(int id)
    {
        bool deleted = await _enrollmentService.DeleteOne(id);
        return deleted ? Ok() : NotFound();
    }
    
    /// <summary>
    /// Get the list of course a student is enroll in.
    /// JWT token is used so no need of student id.
    /// </summary>
    /// <returns>Course a student is enroll in</returns>
    [HttpGet("my_courses")]
    public IEnumerable<object> GetCourseOf()
    {
        return _enrollmentService.GetCourseOf().Select(course => new LiteCourseDTO(course));
    }
}