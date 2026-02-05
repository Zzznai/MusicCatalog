using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class RecordLabelService : BaseService
{
    public RecordLabelService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<RecordLabel>> GetAll()
    {
        return await _context.RecordLabels
            .Include(r => r.Artists)
            .ToListAsync();
    }

    public async Task<RecordLabel?> GetById(int id)
    {
        return await _context.RecordLabels
            .Include(r => r.Artists)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<RecordLabel> Create(RecordLabel recordLabel)
    {
        _context.RecordLabels.Add(recordLabel);
        await _context.SaveChangesAsync();
        return recordLabel;
    }

    public async Task<RecordLabel?> Update(int id, RecordLabel recordLabel)
    {
        var existing = await _context.RecordLabels.FindAsync(id);
        if (existing == null) return null;

        existing.Name = recordLabel.Name;
        existing.BasedIn = recordLabel.BasedIn;
        existing.FoundedYear = recordLabel.FoundedYear;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var recordLabel = await _context.RecordLabels.FindAsync(id);
        if (recordLabel == null) return false;

        _context.RecordLabels.Remove(recordLabel);
        await _context.SaveChangesAsync();
        return true;
    }
}
