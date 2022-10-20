using Microsoft.AspNetCore.Mvc;
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
}