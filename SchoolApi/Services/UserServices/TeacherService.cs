using SchoolApi.DTO.UserDTO;
using SchoolApi.Models.UserModels;

namespace SchoolApi.Services.UserServices;

public class TeacherService
{
    private readonly AuthService _authService;

    public TeacherService(AuthService authService)
    {
        _authService = authService;
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
}