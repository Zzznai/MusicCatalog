using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class AlbumService : BaseService
{
    public AlbumService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Album>> GetAll()
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Songs)
            .Include(a => a.Moods)
            .ToListAsync();
    }

    public async Task<Album?> GetById(int id)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Songs)
            .Include(a => a.Moods)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Album> Create(Album album)
    {
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
        return album;
    }

    public async Task<Album?> Update(int id, Album album)
    {
        var existing = await _context.Albums.FindAsync(id);
        if (existing == null) return null;

        existing.Name = album.Name;
        existing.Description = album.Description;
        existing.ArtistId = album.ArtistId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null) return false;

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
        return true;
    }
}
