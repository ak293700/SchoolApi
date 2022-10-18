namespace SchoolApi.DTO;

public class RegisterUserDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public RegisterUserDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }
}