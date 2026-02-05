using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class ArtistAwardService : BaseService
{
    public ArtistAwardService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<ArtistAward>> GetAll()
    {
        return await _context.ArtistAwards
            .Include(aa => aa.Artist)
            .Include(aa => aa.Award)
            .ToListAsync();
    }

    public async Task<ArtistAward?> GetById(int artistId, int awardId)
    {
        return await _context.ArtistAwards
            .Include(aa => aa.Artist)
            .Include(aa => aa.Award)
            .FirstOrDefaultAsync(aa => aa.ArtistId == artistId && aa.AwardId == awardId);
    }

    public async Task<List<ArtistAward>> GetByArtistId(int artistId)
    {
        return await _context.ArtistAwards
            .Include(aa => aa.Award)
            .Where(aa => aa.ArtistId == artistId)
            .ToListAsync();
    }

    public async Task<List<ArtistAward>> GetByAwardId(int awardId)
    {
        return await _context.ArtistAwards
            .Include(aa => aa.Artist)
            .Where(aa => aa.AwardId == awardId)
            .ToListAsync();
    }

    public async Task<ArtistAward> Create(ArtistAward artistAward)
    {
        _context.ArtistAwards.Add(artistAward);
        await _context.SaveChangesAsync();
        return artistAward;
    }

    public async Task<ArtistAward?> Update(int artistId, int awardId, ArtistAward artistAward)
    {
        var existing = await _context.ArtistAwards
            .FirstOrDefaultAsync(aa => aa.ArtistId == artistId && aa.AwardId == awardId);
        
        if (existing == null) return null;

        existing.Year = artistAward.Year;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int artistId, int awardId)
    {
        var artistAward = await _context.ArtistAwards
            .FirstOrDefaultAsync(aa => aa.ArtistId == artistId && aa.AwardId == awardId);
        
        if (artistAward == null) return false;

        _context.ArtistAwards.Remove(artistAward);
        await _context.SaveChangesAsync();
        return true;
    }
}
