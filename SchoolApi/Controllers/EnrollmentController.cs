using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO;
using SchoolApi.Models;
using SchoolApi.Services;

namespace SchoolApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class EnrollmentController : ControllerBase
{
    private readonly EnrollmentService _enrollmentService;

    public EnrollmentController(EnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }
    
    [HttpGet]
    public async Task<IEnumerable<LiteEnrollmentDTO>> GetAll()
    {
        return (await _enrollmentService.GetAll()).Select(e => new LiteEnrollmentDTO(e));
    }
    
    [HttpGet("{id}")]
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
    public async Task<IActionResult> DeleteOne(int id)
    {
        bool deleted = await _enrollmentService.DeleteOne(id);
        return deleted ? Ok() : NotFound();
    }
}