using SchoolApi.DTO.UserDTO;
using SchoolApi.Models.CourseModels;

namespace SchoolApi.DTO.CourseDTO;

public class CourseDetailDTO
{
    public CourseDetailDTO(int id, string name, LiteTeacherDTO teacher, int price)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Teacher = teacher ?? throw new ArgumentNullException(nameof(teacher));
        Price = price;
    }

    public CourseDetailDTO(Course course, CourseDetail courseDetail)
        : this(course.Id, course.Name, new LiteTeacherDTO(course.Teacher), courseDetail.Price)
    {
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public LiteTeacherDTO Teacher { get; set; }
    public int Price { get; set; }
}