namespace SchoolApi.Models.User;

public class Student : User
{
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}