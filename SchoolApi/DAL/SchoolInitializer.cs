using SchoolApi.Models;

namespace SchoolApi.DAL;
using System.Collections.Generic;

public class SchoolInitializer
{
    private readonly SchoolApiContext _context;
    
    public SchoolInitializer(SchoolApiContext schoolApiContext)
    {
        _context = schoolApiContext;
    }
    
    public void Seed()
        {
            List<User> students = new List<User>
            {
                new() {Email="alexandre.akdeniz@free.fr"},
                new() {Email="alexcharbo@orange.fr"},
                new() {Email="enzo.andreose20@gmail.com"},
                new() {Email="hugo.losa@outlook.com"},
                new() {Email="kevy.lairet@live.fr"},
            };

            students.ForEach(s => _context.Users.Add(s));
            _context.SaveChanges();
            
            List<Course> courses = new List<Course>
            {
                new() {Name = "C#"},
                new() {Name = "Java"},
                new() {Name = "C"},
                new() {Name = "Rust"},
                new() {Name = "HTML"},
                new() {Name = "CSS"},
            };
            courses.ForEach(s => _context.Courses.Add(s));
            _context.SaveChanges();
            
            List<Enrollment> enrollments = new List<Enrollment>
            {
                new() {CourseId = 1, UserId = 1},
                new() {CourseId = 3, UserId = 2},
                new() {CourseId = 2, UserId = 4},
                new() {CourseId = 1, UserId = 5},
                new() {CourseId = 5, UserId = 1},
                new() {CourseId = 5, UserId = 3},
            };
            enrollments.ForEach(s => _context.Enrollments.Add(s));
            _context.SaveChanges();
        }
}