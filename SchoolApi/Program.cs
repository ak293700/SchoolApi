using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolApi.DAL;
using SchoolApi.Services;
using SchoolApi.Services.UserServices;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolApiContext>(options =>
{
    // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    string connectionString = builder.Configuration.GetConnectionString("MySqlConnection")
                              ?? throw new ArgumentException("Connection string not found");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddTransient<SchoolInitializer>();
builder.Services.AddScoped<CourseService>(); // Make CourseService injectable
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>();
builder.Services
    .AddHttpContextAccessor(); // Allow to access HttpContext in services (for example to get current user roles)

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolApi", Version = "v1" });

    // Allow swagger to handle JWT Bearer tokens and authorization
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    // That is a need for authorization in swagger to
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


// Add authentication with jwt bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8
                    .GetBytes(builder.Configuration
                        .GetSection("AppSettings:AuthToken").Value ?? throw new Exception("Token not found"))
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else // dev mod
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Uncomment to go back to the initial state
    // SchoolApiContext.DropCreateDatabase(app);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();