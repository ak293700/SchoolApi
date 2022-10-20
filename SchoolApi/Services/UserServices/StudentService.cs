using SchoolApi.DTO;
using SchoolApi.Models.UserModels;

namespace SchoolApi.Services.UserServices;

public class StudentService
{
    private readonly AuthService _authService;

    public StudentService(AuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Register(RegisterUserDTO user)
    {
        Student student = new Student
        {
            Email = user.Email
        };

        return await _authService.Register(user, student);
    }
}