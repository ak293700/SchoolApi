using SchoolApi.Models;

namespace SchoolApi.DTO.EnrollmentDTO;

public class LiteEnrollmentDTO
{
    public LiteEnrollmentDTO(int id, int courseId, int studentId)
    {
        Id = id;
        CourseId = courseId;
        StudentId = studentId;
    }

    public LiteEnrollmentDTO(Enrollment enrollment) : this(enrollment.Id, enrollment.CourseId, enrollment.StudentId)
    {
    }

    public int Id { get; set; }

    public int CourseId { get; set; }

    public int StudentId { get; set; }

    public Enrollment ToModel()
    {
        return new Enrollment()
        {
            CourseId = CourseId,
            StudentId = StudentId
        };
    }
}