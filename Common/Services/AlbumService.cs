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
        return await _context.Albums.Include(a=>a.Artist).
        Include(a=>a.Songs).Include(a=>a.Moods).ToListAsync();
    }

    public async Task<Album?> GetById(int Id)
    {
        return await _context.Albums.FirstOrDefaultAsync(a=>a.Id==Id);
    }

    public async Task<Album> Create(Album album)
    {
        await _context.Albums.AddAsync(album);
        _context.SaveChangesAsync();
        return album;
    }

    public async Task<bool> Delete(int id)
    {
        Album album=await _context.Albums.FindAsync(id);
        if(album == null)
        {
            return false;
        }
        else
        {
            _context.Remove(album);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<Album?> Update(int id, Album album)
    {
        var existing =await _context.Albums.FindAsync(id);
        if(album==null)
        {
            return null;
        }
        else
        {
            existing.Name=album.Name;
            existing.Description=album.Description;
            await _context.SaveChangesAsync();
            return existing;
        }
    }
 
 
    
}
