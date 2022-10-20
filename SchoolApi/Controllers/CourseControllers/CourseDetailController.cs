using Microsoft.AspNetCore.Mvc;
using SchoolApi.DTO.CourseDTO;
using SchoolApi.Services.CourseServices;

namespace SchoolApi.Controllers.CourseControllers;

[Route("api/[controller]")]
[ApiController]
public class CourseDetailController : ControllerBase
{
    private readonly CourseDetailService _courseDetailService;

    public CourseDetailController(CourseDetailService courseDetailService)
    {
        _courseDetailService = courseDetailService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDetailDTO>> GetCourseDetail(int id)
    {
        try
        {
            return Ok(await _courseDetailService.GetCourseDetail(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}