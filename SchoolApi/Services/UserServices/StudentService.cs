using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SchoolApi.DAL;
using SchoolApi.DTO.CourseDTO;
using SchoolApi.DTO.UserDTO;
using SchoolApi.Models.UserModels;

namespace SchoolApi.Services.UserServices;

public class StudentService
{
    private readonly AuthService _authService;
    private readonly SchoolApiContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StudentService(SchoolApiContext context, AuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Register(RegisterUserDTO user)
    {
        Student student = new Student
        {
            Email = user.Email
        };

        return await _authService.Register(user, student);
    }

    public async Task<List<LiteCourseDTO>> GetCourses()
    {
        if (_httpContextAccessor.HttpContext == null)
            throw new Exception("Http context is null");

        string claim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? throw new Exception("No claim found");

        int studentId = int.Parse(claim);
        List<LiteCourseDTO> courses =
            await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .Select(e => new LiteCourseDTO(e.CourseId, e.Course.Name))
                .ToListAsync();

        return courses;
    }
}