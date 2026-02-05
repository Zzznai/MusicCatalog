using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class MoodService : BaseService
{
    public MoodService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Mood>> GetAll()
    {
        return await _context.Moods
            .Include(m => m.Albums)
            .ToListAsync();
    }

    public async Task<Mood?> GetById(int id)
    {
        return await _context.Moods
            .Include(m => m.Albums)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mood> Create(Mood mood)
    {
        _context.Moods.Add(mood);
        await _context.SaveChangesAsync();
        return mood;
    }

    public async Task<Mood?> Update(int id, Mood mood)
    {
        var existing = await _context.Moods.FindAsync(id);
        if (existing == null) return null;

        existing.Name = mood.Name;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var mood = await _context.Moods.FindAsync(id);
        if (mood == null) return false;

        _context.Moods.Remove(mood);
        await _context.SaveChangesAsync();
        return true;
    }
}
