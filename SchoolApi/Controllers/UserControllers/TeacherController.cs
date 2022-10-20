using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO;
using SchoolApi.DTO.UserDTO;
using SchoolApi.Services.UserServices;

namespace SchoolApi.Controllers.UserControllers;

[Route("api/[controller]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly TeacherService _teacherService;

    public TeacherController(TeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDTO user)
    {
        bool result = await _teacherService.Register(user);
        if (result == false)
            return BadRequest("Email already registered");

        return Ok();
    }

    [HttpGet("courses")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<List<LiteCourseDTO>>> GetCourses()
    {
        try
        {
            List<LiteCourseDTO> courses = await _teacherService.GetCourses();
            return Ok(courses);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}