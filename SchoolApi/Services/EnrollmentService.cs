using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using SchoolApi.DAL;
using SchoolApi.Models;
using SchoolApi.Models.CourseModels;
using SchoolApi.Models.UserModels;
using SchoolApi.Services.CourseServices;

namespace SchoolApi.Services;

public class EnrollmentService
{
    private readonly SchoolApiContext _context;
    private readonly CourseService _courseService;
    private readonly EmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public EnrollmentService(SchoolApiContext context, IHttpContextAccessor httpContextAccessor,
        CourseService courseService, EmailService emailService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _courseService = courseService;
        _emailService = emailService;
    }

    public async Task<IEnumerable<Enrollment>> GetAll()
    {
        return await _context.Enrollments.ToListAsync();
    }

    public async Task<Enrollment?> GetOne(int id)
    {
        return await _context.Enrollments.FindAsync(id);
    }

    public async Task<Enrollment?> CreateOne(int courseId, int studentId)
    {
        // If the user or course doesn't exist, throw an exception
        if (!await _context.Users.AnyAsync(s => s.Id == studentId)
            || !await _context.Courses.AnyAsync(c => c.Id == courseId))
            return null;

        // if the enrollment already exists
        // look for an enrollment with the same course and user
        Enrollment? model =
            await _context.Enrollments.FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);

        if (model != null) // should add a way to return if it has been created or if it already existed
            return model;

        // Send an email to the user to confirm the enrollment
        _emailService.SendEmail(studentId, "Enrollment Confirmation",
            new TextPart(TextFormat.Html) { Text = NewEnrollmentMessage(studentId, courseId) });

        model = new Enrollment { CourseId = courseId, StudentId = studentId };

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

    private string NewEnrollmentMessage(int userId, int courseId)
    {
        User? user = _context.Users.Find(userId);
        Course? course = _context.Courses.Find(courseId);

        if (user == null || course == null)
            return "An error occurred while trying to enroll you in a course.";

        string username = user.Email.Split('@')[0];
        string courseName = course.Name;

        return
            $@"
    <h1 style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">Bonjour {username}</h1>
    <p style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: .6rem 0;"">
        On voulais vous remercier pour votre inscription au cours de <strong style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">{courseName}</strong>
    </p>

    <section style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; padding: 2rem 1rem; width: 100%; border-radius: 1rem; background-color: white;"">
        <h2 style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size: 2rem; font-weight: 700; margin: 0 0 2rem 0;"">Qu’est-ce qu’un radiateur à inertie ?</h2>
        <div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; display: flex; justify-content: center; align-items: center;"" class=""section-content"">
            <div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 20px;"">
                <p style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: .6rem 0; font-size: 1rem; font-weight: 1000;"">
                    {courseName} est un langage créé par Microsoft en 2000. Il est basé sur le langage Java et est
                    utilisé pour
                    créer des applications Windows, Web et mobiles. Il est également utilisé pour créer des jeux vidéo.
                </p>
                <p style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: .6rem 0; font-size: 1rem; font-weight: 1000;"">
                    Ce cours de {courseName} pour les débutants est composé de 5 chapitres et d’un quiz.
                </p>
            </div>
            <img src=""data:image/png;base64,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"" style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; height: 200px; width: auto;"">
        </div>
        <center style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">
            <a href=""https://learn.microsoft.com/fr-fr/dotnet/csharp/"" target=""_blank"" style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"" class=""info"">
                <b style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">Documentation {courseName} officiel</b>
            </a>
        </center>
    </section>
    <h2 style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 3rem; font-size: 2rem;"">Merci de votre confiance</h2>";
    }
}