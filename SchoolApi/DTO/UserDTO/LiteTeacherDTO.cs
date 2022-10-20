using SchoolApi.Models.UserModels;

namespace SchoolApi.DTO.UserDTO;

public class LiteTeacherDTO : LiteUserDTO
{
    public LiteTeacherDTO(int id, string email, int salary) : base(id, email)
    {
        Salary = salary;
    }

    public LiteTeacherDTO(Teacher user) : base(user)
    {
        Salary = Salary;
    }

    public int Salary { get; set; }
}