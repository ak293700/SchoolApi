using Microsoft.EntityFrameworkCore;
using SchoolApi.DAL;
using SchoolApi.Models;

namespace SchoolApi.Services;

public class CourseService
{
    private readonly SchoolApiContext _context;
    
    public CourseService(SchoolApiContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Course>> GetAll()
    {
        return await _context.Courses.ToListAsync();
    }
    
    public async Task<Course?> GetOne(int id)
    {
        Course? course = await _context.Courses.FirstOrDefaultAsync(c => c.Id.Equals(id));
        if (course == null)
            return null;
        return course;
    }
    
    /// <summary>
    ///  Create a new course. Check if an identical course already exists.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Return the corresponding course.</returns>
    public async Task<Course> CreateOne(string name)
    {
        // Check that the course does not already exists
        Course? course = await _context.Courses.FirstOrDefaultAsync(c => c.Name.Equals(name));
        if (course != null) // if so redirect to get it 
            return course;

        course = new Course {Name = name};
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
            
        return course;
    }
    
    /// <summary>
    /// Delete a course
    /// </summary>
    /// <param name="id">The id of the course you want to delete</param>
    /// <returns>If the course has been found and well deleted</returns>
    public async Task<bool> DeleteOne(int id)
    {
        Course? course = await _context.Courses.FindAsync(id);
        if (course == null)
            return false;
            
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return true;
    }
}