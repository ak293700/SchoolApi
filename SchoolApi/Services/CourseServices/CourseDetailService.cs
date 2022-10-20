using SchoolApi.DAL;
using SchoolApi.DTO.CourseDTO;
using SchoolApi.Models.CourseModels;

namespace SchoolApi.Services.CourseServices;

public class CourseDetailService
{
    private readonly SchoolApiContext _context;

    public CourseDetailService(SchoolApiContext context)
    {
        _context = context;
    }

    public async Task<CourseDetailDTO> GetCourseDetail(int id)
    {
        Course? course = await _context.Courses.FindAsync(id);
        if (course == null)
            throw new ArgumentException($"No course for id: {id}");

        CourseDetail? courseDetail = await _context.CourseDetails.FindAsync(id);
        if (courseDetail == null)
            throw new ArgumentException($"No course detail for id: {id}");

        return new CourseDetailDTO(course, courseDetail);
    }
}