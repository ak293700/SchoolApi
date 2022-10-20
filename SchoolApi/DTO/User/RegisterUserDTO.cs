using SchoolApi.Models.UserModels;

namespace SchoolApi.DTO;

public class RegisterUserDTO
{
    public RegisterUserDTO(string email, string password, UserType userType)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}