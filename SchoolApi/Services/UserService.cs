using Microsoft.EntityFrameworkCore;
using SchoolApi.DAL;
using SchoolApi.Models;

namespace SchoolApi.Services;

public class UserService
{
    private readonly SchoolApiContext _context;
    
    public UserService(SchoolApiContext schoolApiContext)
    {
        _context = schoolApiContext;
    }
    
    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<User?> GetOne(int id)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(c => c.Id.Equals(id));
        if (user == null)
            return null;
        return user;
    }
    
    /// <summary>
    ///  Create a new User.
    /// </summary>
    /// <param name="email">The email you want</param>
    /// <param name="password">The password you want</param>
    /// <returns>Return the corresponding user.</returns>
    public async Task<User> CreateOne(string email, string password)
    {
        User user = new User {Email = email};
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
            
        return user;
    }
    
    /// <summary>
    /// Delete a user.
    /// </summary>
    /// <param name="id">The id of the user you want to delete</param>
    /// <returns>If the user has been found and well deleted</returns>
    public async Task<bool> DeleteOne(int id)
    {
        User? user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;
            
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}