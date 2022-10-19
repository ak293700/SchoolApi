using SchoolApi.Models;
using SchoolApi.Models.User;

namespace SchoolApi.DTO;

public class LiteUserDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    
    public LiteUserDTO(int id, string email)
    {
        Id = id;
        Email = email;
    }
    
    public LiteUserDTO(User user) : this(user.Id, user.Email)
    {}
}