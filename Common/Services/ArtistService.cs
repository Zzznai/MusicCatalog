using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class ArtistService : BaseService
{
    public ArtistService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Artist>> GetAll()
    {
        return await _context.Artists
            .Include(a => a.RecordLabel)
            .Include(a => a.Albums)
            .Include(a => a.Songs)
            .Include(a => a.Awards)
            .ToListAsync();
    }

    public async Task<Artist?> GetById(int id)
    {
        return await _context.Artists
            .Include(a => a.RecordLabel)
            .Include(a => a.Albums)
            .Include(a => a.Songs)
            .Include(a => a.Awards)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Artist?> Create(Artist artist)
    {
        var recordLabel = await _context.RecordLabels.FindAsync(artist.RecordLabelId);

        if (recordLabel == null) return null;
        
        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();
        
        return await _context.Artists
            .Include(a => a.RecordLabel)
            .Include(a => a.Albums)
            .Include(a => a.Songs)
            .Include(a => a.Awards)
            .FirstOrDefaultAsync(a => a.Id == artist.Id);
    }

    public async Task<Artist?> Update(int id, Artist artist)
    {
        var existing = await _context.Artists.FindAsync(id);
        if (existing == null) return null;

        var recordLabel = await _context.RecordLabels.FindAsync(artist.RecordLabelId);
        if (recordLabel == null) return null;

        existing.StageName = artist.StageName;
        existing.Description = artist.Description;
        existing.RecordLabelId = artist.RecordLabelId;

        await _context.SaveChangesAsync();
        
        return await _context.Artists
            .Include(a => a.RecordLabel)
            .Include(a => a.Albums)
            .Include(a => a.Songs)
            .Include(a => a.Awards)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> Delete(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return false;

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();
        return true;
    }
}
