using System.Security.Cryptography;
using System.Text;
using Common.Enums;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class UserService : BaseService
{
    public UserService(ApplicationDbContext context) : base(context)
    {
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        var computedHash = HashPassword(password);
        return computedHash == storedHash;
    }

    public async Task<List<User>> GetAll()
    {
        return await _context.Users
            .Include(u => u.Playlists)
            .ToListAsync();
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users
            .Include(u => u.Playlists)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users
            .Include(u => u.Playlists)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> Create(string username, string password)
    {
        var hash = HashPassword(password);
        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            Role = Role.Consumer
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Authenticate(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return null;

        return VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    public async Task<bool> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
