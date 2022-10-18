using SchoolApi.Models;

namespace SchoolApi.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

public class SchoolInitializer
{
    private readonly SchoolApiContext _context;
    
    public SchoolInitializer(SchoolApiContext schoolApiContext)
    {
        _context = schoolApiContext;
    }
    
    public void Seed()
        {
            var students = new List<User>
            {
                new User{Email="alexandre.akdeniz@free.fr"},
                new User{Email="alexcharbo@orange.fr"},
                new User{Email="enzo.andreose20@gmail.com"},
                new User{Email="hugo.losa@outlook.com"},
                new User{Email="kevy.lairet@live.fr"},
            };

            students.ForEach(s => _context.Users.Add(s));
            _context.SaveChanges();
            
            var courses = new List<Course>
            {
                new Course{Name = "C#"},
                new Course{Name = "Java"},
                new Course{Name = "C"},
                new Course{Name = "Rust"},
                new Course{Name = "HTML"},
                new Course{Name = "CSS"},
            };
            courses.ForEach(s => _context.Courses.Add(s));
            _context.SaveChanges();
            var enrollments = new List<Enrollment>
            {
                new Enrollment{CourseId = 1, UserId = 1},
                new Enrollment{CourseId = 3, UserId = 2},
                new Enrollment{CourseId = 2, UserId = 4},
                new Enrollment{CourseId = 1, UserId = 5},
                new Enrollment{CourseId = 5, UserId = 1},
                new Enrollment{CourseId = 5, UserId = 3},
            };
            enrollments.ForEach(s => _context.Enrollments.Add(s));
            _context.SaveChanges();
        }
}