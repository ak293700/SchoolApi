using SchoolApi.Models.UserModels;

namespace SchoolApi.DTO;

public class LiteUserDTO
{
    public LiteUserDTO(int id, string email)
    {
        Id = id;
        Email = email;
    }

    public LiteUserDTO(User user) : this(user.Id, user.Email)
    {
    }

    public int Id { get; set; }
    public string Email { get; set; }
}