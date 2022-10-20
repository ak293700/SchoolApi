using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SchoolApi.DAL;
using SchoolApi.DTO;
using SchoolApi.DTO.UserDTO;
using SchoolApi.Models.CourseModels;
using SchoolApi.Models.UserModels;

namespace SchoolApi.Services.UserServices;

public class TeacherService
{
    private readonly AuthService _authService;
    private readonly SchoolApiContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TeacherService(SchoolApiContext context, AuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Register(RegisterUserDTO user)
    {
        Teacher student = new Teacher
        {
            Email = user.Email,
            Salary = 0
        };

        return await _authService.Register(user, student);
    }

    public async Task<List<LiteCourseDTO>> GetCourses()
    {
        if (_httpContextAccessor.HttpContext == null)
            throw new Exception("Http context is null");

        string claim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? throw new Exception("No claim found");

        int teacherId = int.Parse(claim);

        List<Course> courses = await _context.Courses
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();

        return courses.Select(c => new LiteCourseDTO(c)).ToList();
    }
}