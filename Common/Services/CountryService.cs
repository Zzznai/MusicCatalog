using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class CountryService : BaseService
{
    public CountryService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Country>> GetAll()
    {
        return await _context.Countries
            .Include(c => c.RecordLabels)
            .ToListAsync();
    }

    public async Task<Country?> GetById(int id)
    {
        return await _context.Countries
            .Include(c => c.RecordLabels)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Country> Create(Country country)
    {
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();
        
        return await _context.Countries
            .Include(c => c.RecordLabels)
            .FirstAsync(c => c.Id == country.Id);
    }

    public async Task<Country?> Update(int id, string name)
    {
        var existing = await _context.Countries.FindAsync(id);
        if (existing == null) return null;

        existing.Name = name;

        await _context.SaveChangesAsync();
        
        return await _context.Countries
            .Include(c => c.RecordLabels)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> Delete(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return false;

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
        return true;
    }
}
