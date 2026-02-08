using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class GenreService : BaseService
{
    public GenreService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Genre>> GetAll()
    {
        return await _context.Genres
            .Include(g => g.Songs)
            .ToListAsync();
    }

    public async Task<Genre?> GetById(int id)
    {
        return await _context.Genres
            .Include(g => g.Songs)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Genre?> Create(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        
        return await _context.Genres
            .Include(g => g.Songs)
            .FirstOrDefaultAsync(g => g.Id == genre.Id);
    }

    public async Task<Genre?> Update(int id, String name)
    {
        var existing = await _context.Genres.FindAsync(id);
        if (existing == null) return null;

        existing.Name = name;

        await _context.SaveChangesAsync();
        
        return await _context.Genres
            .Include(g => g.Songs)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<bool> Delete(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre == null) return false;

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return true;
    }
}
