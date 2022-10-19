using System.ComponentModel.DataAnnotations.Schema;
using SchoolApi.Models.User;

namespace SchoolApi.Models;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TeacherId { get; set; }
    
    public Teacher Teacher { get; set; }
    
    // Change noting in the database, allow to access the related data more easily
    public ICollection<Enrollment> Enrollments { get; set; }
}