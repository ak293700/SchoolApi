using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolApi.DAL;
using SchoolApi.DTO.UserDTO;
using SchoolApi.Models.UserModels;

namespace SchoolApi.Services;

public class AuthService
{
    private readonly string _authToken;
    private readonly SchoolApiContext _context;

    public AuthService(SchoolApiContext context, IConfiguration configuration)
    {
        _context = context;
        _authToken = configuration.GetSection("AppSettings:AuthToken").Value ??
                     throw new Exception("No AuthToken in find");
    }

    public async Task<bool> Register(RegisterUserDTO request, User user)
    {
        // Next we need to check if the user already exists

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        // Set password information
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

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
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
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
    private string CreateToken(User user)
    {
        List<Claim> claims = GetClaims(user);

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

    private List<Claim> GetClaims(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
        };

        switch (user)
        {
            case Student:
                claims.Add(new Claim(ClaimTypes.Role, UserType.Student.ToString()));
                break;
            case Teacher:
                claims.Add(new Claim(ClaimTypes.Role, UserType.Teacher.ToString()));
                break;
        }

        return claims;
    }
}