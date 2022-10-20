using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;
using SchoolApi.Models.CourseModels;
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

    public DbSet<CourseDetail> CourseDetails { get; set; }

    public static void DropCreateDatabase(WebApplication app)
    {
        // Create a scope to be able to inject a SchoolApiContext instance 
        using (var scope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            // Get the instance
            SchoolApiContext? schoolApiContext = scope?.ServiceProvider.GetRequiredService<SchoolApiContext>();
            if (schoolApiContext == null)
                throw new Exception("Could not get SchoolApiContext");

            schoolApiContext.Database.EnsureDeleted();
            schoolApiContext.Database.EnsureCreated();
            scope?.ServiceProvider.GetService<SchoolInitializer>()?.Seed();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Create the discriminator for inheritance models
        modelBuilder.Entity<User>()
            .HasDiscriminator<UserType>("UserType")
            .HasValue<Student>(UserType.Student)
            .HasValue<Teacher>(UserType.Teacher);

        /*modelBuilder.Entity<CourseDetail>()
            .HasOne(c => c.Course)
            .WithOne()
            .HasForeignKey<CourseDetail>(c => c.Id)
            .HasPrincipalKey<CourseDetail>(c => c.Id)
            .IsRequired();*/

        // modelBuilder.Entity<CourseDetail>()
        // .Property(c => c.Id)
        // .HasC


        base.OnModelCreating(modelBuilder);
    }
}