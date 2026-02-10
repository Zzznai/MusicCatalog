using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class AlbumService : BaseService
{
    public AlbumService(ApplicationDbContext context) : base(context)
    {
    }

    public static bool HasSong(Album album, int songId)
        => album.Songs.Any(s => s.Id == songId);

    public static bool HasMood(Album album, int moodId)
        => album.Moods.Any(m => m.Id == moodId);

    public async Task<List<Album>> GetAll()
    {
        return await _context.Albums.Include(a=>a.Songs).Include(a=>a.Moods).Include(a=>a.Artist).
        Include(a=>a.Songs).Include(a=>a.Moods).ToListAsync();
    }

    public async Task<Album?> GetById(int Id)
    {
        return await _context.Albums.Include(a=>a.Songs).Include(a=>a.Moods).Include(a=>a.Artist).FirstOrDefaultAsync(a=>a.Id==Id);
    }

    public async Task<Album?> Create(Album album)
    {
        var artist = await _context.Artists.FindAsync(album.ArtistId);

        if (artist == null) return null;

        await _context.Albums.AddAsync(album);
        await _context.SaveChangesAsync();

        return await _context.Albums.Include(a => a.Artist).FirstOrDefaultAsync(a => a.Id == album.Id);
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
        var existing = await _context.Albums.FindAsync(id);
        if (existing == null)
        {
            return null;
        }

        var artist = await _context.Artists.FindAsync(album.ArtistId);
        if (artist == null) return null;

        existing.Name = album.Name;
        existing.Description = album.Description;
        existing.ArtistId = album.ArtistId;
        await _context.SaveChangesAsync();
        
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Songs)
            .Include(a => a.Moods)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> AddSong(int albumId, int songId)
    {
        var album = await _context.Albums.Include(a=>a.Songs).FirstOrDefaultAsync(a=>a.Id==albumId);
        if(album == null) return false;

        var song = await _context.Songs.FindAsync(songId);
        if(song == null) return false;

        if(album.Songs.Any(s=>s.Id == songId)) return false;

        album.Songs.Add(song);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveSong(int albumId, int songId)
    {
        var album = await _context.Albums.Include(a=>a.Songs).FirstOrDefaultAsync(a=>a.Id==albumId);
        if(album == null) return false;

        var song = await _context.Songs.FindAsync(songId);
        if(song == null) return false;

        if(album.Songs.Any(s=>s.Id == songId)) 
        {
            album.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        return false;

       
    }

    public async Task<bool> AddMood(int albumId, int moodId)
    {
        var album = await _context.Albums.Include(a=>a.Moods).FirstOrDefaultAsync(a=>a.Id==albumId);
        if(album == null) return false;

        var mood = await _context.Moods.FindAsync(moodId);
        if(mood == null) return false;

        if(album.Moods.Any(m=>m.Id == moodId)) return false;

        album.Moods.Add(mood);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveMood(int albumId, int moodId)
    {
        var album = await _context.Albums.Include(a=>a.Moods).FirstOrDefaultAsync(a=>a.Id==albumId);
        if(album == null) return false;

        var mood = await _context.Moods.FindAsync(moodId);
        if(mood == null) return false;

        if(album.Moods.Any(m=>m.Id == moodId)) 
        {
            album.Moods.Remove(mood);
            await _context.SaveChangesAsync();
            return true;
        }
        else return false;

       
    }
 
}
