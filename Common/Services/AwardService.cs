using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class AwardService : BaseService
{
    public AwardService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Award>> GetAll()
    {
        return await _context.Awards
            .Include(a => a.ArtistAwards)
                .ThenInclude(aa => aa.Artist)
            .ToListAsync();
    }

    public async Task<Award?> GetById(int id)
    {
        return await _context.Awards
            .Include(a => a.ArtistAwards)
                .ThenInclude(aa => aa.Artist)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Award> Create(Award award)
    {
        _context.Awards.Add(award);
        await _context.SaveChangesAsync();
        return award;
    }

    public async Task<Award?> Update(int id, string name)
    {
        var existing = await _context.Awards.FindAsync(id);
        if (existing == null) return null;

        existing.Name = name;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var award = await _context.Awards.FindAsync(id);
        if (award == null) return false;

        _context.Awards.Remove(award);
        await _context.SaveChangesAsync();
        return true;
    }
}
