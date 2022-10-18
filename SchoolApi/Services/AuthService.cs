using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolApi.DAL;
using SchoolApi.DTO;
using SchoolApi.Models;

namespace SchoolApi.Services;

public class AuthService
{
    private readonly SchoolApiContext _context;
    private readonly string _authToken;
    
    public AuthService(SchoolApiContext context, IConfiguration configuration)
    {
        _context = context;
        _authToken = configuration.GetSection("AppSettings:AuthToken").Value ?? throw new Exception("No AuthToken in find");
    }
    
    public async Task<bool> Register(RegisterUserDTO request)
    {
        // Next we need to check if the user already exists
        
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        // So we create the new user
        User user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        
        // And add it to the database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    /// <summary>
    /// Create a combo of password hash and salt
    /// </summary>
    /// <param name="password">The password you want to hash</param>
    /// <param name="passwordHash">The password will be stored in it</param>
    /// <param name="passwordSalt">The password salt will be stored in it</param>
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (HMACSHA512 hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key; // Already generate a random salt for us
            // Hash the password with the salt
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); 
        }
    }

    public async Task<string?> Login(RegisterUserDTO registerUser)
    {
        // Check if the user exists
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerUser.Email);
        if (user == null)
            return null;
        
        // Check if the password is correct
        if (!VerifyPasswordHash(registerUser.Password, user.PasswordHash, user.PasswordSalt))
            return null;
        
        return CreateToken(user);
    }
    
    /// <summary>
    /// Check id the given password correspond to the password hash and salt
    /// </summary>
    /// <param name="password">Password you wan to check</param>
    /// <param name="passwordHash">The reference one (hashed)</param>
    /// <param name="passwordSalt">the corresponding salt of passwordHash</param>
    /// <returns>True if the password is correct, else false</returns>
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        // Init hmac with the same key as our password salt
        using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
        {
            // Hash the password with the salt
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            // Compare the two hashes
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    /// <summary>
    /// Create a JWT token for the given user
    /// </summary>
    /// <returns>return the token as a string</returns>
    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Email, user.Email),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_authToken));
        
        // Credentials
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}