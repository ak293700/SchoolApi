using Microsoft.EntityFrameworkCore;
using SchoolApi.DAL;
using SchoolApi.DTO;
using SchoolApi.Models;

namespace SchoolApi.Services;

public class EnrollmentService
{
    
    private readonly SchoolApiContext _context;
    
    public EnrollmentService(SchoolApiContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Enrollment>> GetAll()
    {
        return await _context.Enrollments.ToListAsync();
    }
    
    public async Task<Enrollment?> GetOne(int id)
    {
        return await _context.Enrollments.FindAsync(id);
    }
    
    public async Task<Enrollment?> CreateOne(int courseId, int userId)
    {
        // If the user or course doesn't exist, throw an exception
        if (!await _context.Users.AnyAsync(s => s.Id == userId) 
            || !await _context.Courses.AnyAsync(c => c.Id == courseId))
            return null;
            
        // if the enrollment already exists
        // look for an enrollment with the same course and user
        Enrollment? model = await _context.Enrollments.FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId);
        
        if (model != null) // should add a way to return if it has been created or if it already existed
            return model;
        
        model = new Enrollment {CourseId = courseId, UserId = userId};
        
        _context.Enrollments.Add(model);
        await _context.SaveChangesAsync();

        return model;
    }
    
    public async Task<bool> DeleteOne(int id)
    {
        Enrollment? enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
            return false;
        
        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
        
        return true;
    }
}