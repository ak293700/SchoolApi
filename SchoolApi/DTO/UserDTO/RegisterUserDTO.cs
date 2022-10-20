namespace SchoolApi.DTO.UserDTO;

public class RegisterUserDTO
{
    public RegisterUserDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }
    public string Password { get; set; }
}