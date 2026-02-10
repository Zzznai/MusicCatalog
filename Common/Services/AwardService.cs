using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class AwardService : BaseService
{
    public AwardService(ApplicationDbContext context) : base(context)
    {
    }

    public static bool HasArtist(Award award, int artistId)
        => award.Artists.Any(a => a.Id == artistId);

    public async Task<List<Award>> GetAll()
    {
        return await _context.Awards
            .Include(a => a.Artists)
            .ToListAsync();
    }

    public async Task<Award?> GetById(int id)
    {
        return await _context.Awards
            .Include(a => a.Artists)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Award?> Create(Award award)
    {
        _context.Awards.Add(award);
        await _context.SaveChangesAsync();
        
        return await _context.Awards
            .Include(a => a.Artists)
            .FirstOrDefaultAsync(a => a.Id == award.Id);
    }

    public async Task<Award?> Update(int id, string name, int year)
    {
        var existing = await _context.Awards.FindAsync(id);
        if (existing == null) return null;

        existing.Name = name;
        existing.Year = year;

        await _context.SaveChangesAsync();
        
        return await _context.Awards
            .Include(a => a.Artists)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> Delete(int id)
    {
        var award = await _context.Awards.FindAsync(id);
        if (award == null) return false;

        _context.Awards.Remove(award);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddWinner(int awardId, int artistId)
    {
        var award = await _context.Awards
            .Include(a=>a.Artists)
            .FirstOrDefaultAsync(aw=>aw.Id == awardId);

        if (award == null) return false;

        var artist = await _context.Artists.FindAsync(artistId);
        if(artist == null) return false;


        award.Artists.Add(artist);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveWinner(int awardId, int artistId)
    {
        var award = await _context.Awards
            .Include(a => a.Artists)
            .FirstOrDefaultAsync(aw => aw.Id == awardId);

        if (award == null) return false;

        var artist = award.Artists.FirstOrDefault(a => a.Id == artistId);
        if (artist == null) return false;

        award.Artists.Remove(artist);
        await _context.SaveChangesAsync();

        return true;
    }
}
