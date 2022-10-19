using SchoolApi.Models;

namespace SchoolApi.DTO;

public class LiteEnrollmentDTO
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    
    public int StudentId { get; set; }
    
    public LiteEnrollmentDTO(int id, int courseId, int studentId)
    {
        Id = id;
        CourseId = courseId;
        StudentId = studentId;
    }
    
    public LiteEnrollmentDTO(Enrollment enrollment) : this(enrollment.Id, enrollment.CourseId, enrollment.StudentId)
    {}

    public Enrollment ToModel()
    {
        return new Enrollment()
        {
            CourseId = CourseId,
            StudentId = StudentId
        };
    }
}