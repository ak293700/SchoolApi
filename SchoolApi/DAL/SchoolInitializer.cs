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
                new()
                {
                    Email="alexandre.akdeniz@free.fr", 
                    PasswordHash = Convert.FromBase64String("uVaeI+REvmUsgCsYRkZJ4IVpF4UyM2/t1gOyklg7UEnPkZcUno9/o7+Z1l0FpqkUj8QzXV4nuknfEdZT9HPkoA=="),
                    PasswordSalt =  Convert.FromBase64String("K7ojWxJlethoBwPx612+6RSiulFRLf0wOwbAuMrdBzhiFNaN/y68M9eCoXeiwMfQL/YeAEL+Pa77B50kkaauX++TVi1NEM7hzH6pdtqJJdUC2nBjdtgZjWqx7EaS8tOPfXckiQYNeGTsd2SksG35WMPyN4nviIn2WD8aG+7QUyI=")
                },
                new()
                {
                    Email="alexcharbo@orange.fr",
                    PasswordHash = Convert.FromBase64String("uzXbHTP9X6ktg5cYE9bIq6cmH9oJSHcNNI2oqjysOvP2+1tfu2mtvEHUGOVK4HYhF+Z+Ps6pYRcf+wgd0nxCfw=="),
                    PasswordSalt = Convert.FromBase64String("YKWxNwsDiSn7hzqfw5ninaT94+N/SDksf+Ghu9nxv+w1Y2zPZ+u39JEOX62j8BkV9fUBJ9hJTCrJPgMOgJvMVs/Kt+6KQ9qMjvxqK2GUulg5oj352l5E9OZfaxfjEz31uSgnm1wWbDZsF0mB+gdovenZ9kaWjS2GumcJ75Kztkw=")
                },
                new()
                {
                    Email="enzo.andreose20@gmail.com",
                    PasswordHash = Convert.FromBase64String("UEbwdWPqBWSBbIMeIMVUEbao/IZONG9qgRcvs9qHdeJHHUiIqkgBkU+83QW3/8Ucg7seYZi7DetItICArtAoAA=="),
                    PasswordSalt = Convert.FromBase64String("R+B7aasXSLY+QCkuZSIRXbD8v0Y/1sMtfxFWTyimMFbybwL4zvfyxEUQVoIuZjLF1MHmWM9q1tI3LHZTwi1qiUiR8+B1/kgmg/HWeMkYg+IqGKSnO4WWTAXTSS6N4K4cGhpNHs5eMpenJj/KNYYqNNWkNpftR6xqhrS43eyN6Gw=")
                },
                new()
                {
                    Email="hugo.losa@outlook.com",
                    PasswordHash = Convert.FromBase64String("rFN1FVEBFHsNYDSjADQISs4K/xhnUaCfGguM3cnNpoIkfh7Y3e6OjFS+qL4rq80K8wlsusVyRbvQAaUnbek7fQ=="),
                    PasswordSalt = Convert.FromBase64String("m2rIaieSWqIOW9CHfS/lwDeEyMK59KvpsJEkr3QdJqleEG3s+rrSHg7YS7+QoXIOKY8IC0ChQKXlZ8t0ZJNkFKWGV0DQqXPaTgeQF2tRCS90i2H5OV/M5GznCB78SO9uRkjrV6Ru0hwWFAuXEhHPPdoJ8TV60Y1uo5Im+ttq20g=")
                },
                new()
                {
                    Email="kevy.lairet@live.fr",
                    PasswordHash = Convert.FromBase64String("+U3ZAEUTC/KiG4eK065up86gSIZmELJc3NGdj7w38/QwSuEll+mzyn46jvv5ScKS0g8tcRU0ecjo3SJ9VaIyQA=="),
                    PasswordSalt = Convert.FromBase64String("OOMfHrTSJXC95N9FvnNVeRf4cRacFvVQ9Y2wXlehNcaKygYef1OImAPdrstSsSOJLhvBvnRxKO07lCz8I4s5PFO+TF+xEl6/0MGiRCjj22Mpzaj15jdczQp72jnMKYMGkVn7egkNXAL36oLLtSD8xMzs2FAgQWoEq0pg/awwrUU=")
                },
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