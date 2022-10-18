using SchoolApi.Models;

namespace SchoolApi.DTO;

public class LiteEnrollmentDTO
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    
    public int UserId { get; set; }
    
    public LiteEnrollmentDTO(int id, int courseId, int studentId)
    {
        Id = id;
        CourseId = courseId;
        UserId = studentId;
    }
    
    public LiteEnrollmentDTO(Enrollment enrollment) : this(enrollment.Id, enrollment.CourseId, enrollment.UserId)
    {}

    public Enrollment ToModel()
    {
        return new Enrollment()
        {
            CourseId = CourseId,
            UserId = UserId
        };
    }
}