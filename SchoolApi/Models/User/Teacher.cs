namespace SchoolApi.Models.User;

public class Teacher : User
{
    public int Salary { get; set; }
    
    // Course where the teacher is involved in
    public ICollection<Course> Courses { get; set; }
}