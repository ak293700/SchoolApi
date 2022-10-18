using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
// using DbContext = System.Data.Entity.DbContext;

namespace SchoolApi.DAL;

public class SchoolApiContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }

    // public SchoolApiContext() : base("DefaultConnection")
    public SchoolApiContext(DbContextOptions<SchoolApiContext> options)
        : base(options)
    {}

    public static void DropCreateDatabase(WebApplication app)
    {
        // Create a scope to be able to inject a SchoolApiContext instance 
        using (var scope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            // Get the instance
            SchoolApiContext? schoolApiContext = scope?.ServiceProvider.GetRequiredService<SchoolApiContext>();
            if (schoolApiContext == null)
                throw new Exception("Could not get SchoolApiContext");
            
            schoolApiContext?.Database.EnsureDeleted();
            schoolApiContext?.Database.EnsureCreated();
            scope?.ServiceProvider.GetService<SchoolInitializer>()?.Seed();
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Not necessary because ef6 would have understand by itself
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course) // Explicit that Enrollment.Course is a reference to Course
            .WithMany(c => c.Enrollments) // Explicit that Course.Enrollments is a collection of Enrollment
            .HasForeignKey(e => e.CourseId) // Explicit that CourseId is the foreign key
            .OnDelete(DeleteBehavior.Cascade); // Explicit that when a Course is deleted, all its Enrollments are deleted
            // By default it is set to DeleteBehavior.Cascade
            
        // So I don't do it for Enrollment and User because it is the default behavior
    }
}