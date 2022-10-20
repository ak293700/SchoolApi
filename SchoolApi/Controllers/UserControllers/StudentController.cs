using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO;
using SchoolApi.DTO.UserDTO;
using SchoolApi.Services.UserServices;

namespace SchoolApi.Controllers.UserControllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }


    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDTO user)
    {
        bool result = await _studentService.Register(user);
        if (result == false)
            return BadRequest("Email already registered");

        return Ok();
    }

    [HttpGet("courses")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<List<LiteCourseDTO>>> GetCourses()
    {
        try
        {
            List<LiteCourseDTO> courses = await _studentService.GetCourses();
            return Ok(courses);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}