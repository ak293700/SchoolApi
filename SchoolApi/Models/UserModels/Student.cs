namespace SchoolApi.Models.UserModels;

public class Student : User
{
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}