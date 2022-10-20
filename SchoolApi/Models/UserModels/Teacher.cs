using SchoolApi.Models.CourseModels;

namespace SchoolApi.Models.UserModels;

public class Teacher : User
{
    public int Salary { get; set; }

    // Course where the teacher is involved in
    public virtual ICollection<Course> Courses { get; set; }
}