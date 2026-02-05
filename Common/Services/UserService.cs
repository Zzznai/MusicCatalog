using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class UserService : BaseService
{
    public UserService(ApplicationDbContext context) : base(context)
    {
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

    public async Task<User> Create(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Update(int id, User user)
    {
        var existing = await _context.Users.FindAsync(id);
        if (existing == null) return null;

        existing.Username = user.Username;
        existing.Role = user.Role;

        await _context.SaveChangesAsync();
        return existing;
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
