using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;
using SchoolApi.Models.UserModels;

namespace SchoolApi.DAL;

public class SchoolApiContext : DbContext
{
    public SchoolApiContext(DbContextOptions<SchoolApiContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Enrollment>().ToTable("Enrollments");
        modelBuilder.Entity<Course>().ToTable("Courses");


        // Create the discriminator for inheritance models
        modelBuilder.Entity<User>()
            .HasDiscriminator<UserType>("UserType")
            .HasValue<Student>(UserType.Student)
            .HasValue<Teacher>(UserType.Teacher);


        // Not necessary because ef6 would have understand by itself
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course) // Explicit that Enrollment.Course is a reference to Course
            .WithMany(c => c.Enrollments) // Explicit that Course.Enrollments is a collection of Enrollment
            .HasForeignKey(e => e.CourseId) // Explicit that CourseId is the foreign key
            .OnDelete(DeleteBehavior
                .Cascade); // Explicit that when a Course is deleted, all its Enrollments are deleted
        // By default it is set to DeleteBehavior.Cascade

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // .HasConstraintName("FK_Enrollments_Users_StudentId");

        // modelBuilder.Entity<Enrollment>(entity =>
        // {
        // entity.ToTable("Enrollments");
        // });

        // When an Enrollement is loaded it Course and User are automatically loaded too 
        // modelBuilder.Entity<Enrollment>().Navigation(e => e.Course).AutoInclude();
        // modelBuilder.Entity<Enrollment>().Navigation(e => e.Student).AutoInclude();
    }
}