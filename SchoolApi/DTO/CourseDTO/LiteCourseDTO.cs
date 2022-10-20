using SchoolApi.Models.CourseModels;

namespace SchoolApi.DTO;

public class LiteCourseDTO
{
    public LiteCourseDTO(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public LiteCourseDTO(Course course) : this(course.Id, course.Name)
    {
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public Course ToModel()
    {
        return new Course
        {
            Id = Id,
            Name = Name
        };
    }
}