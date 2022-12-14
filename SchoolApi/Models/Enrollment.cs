using SchoolApi.Models.CourseModels;
using SchoolApi.Models.UserModels;

namespace SchoolApi.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }

    public virtual Course Course { get; set; }
    public virtual Student Student { get; set; }
}