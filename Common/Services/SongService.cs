using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class SongService : BaseService
{
    public SongService(ApplicationDbContext context) : base(context)
    {
    }

    public static bool HasGenre(Song song, int genreId)
        => song.Genres.Any(g => g.Id == genreId);

    public async Task<List<Song>> GetAll()
    {
        return await _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Album)
            .Include(s => s.Genres)
            .ToListAsync();
    }

    public async Task<Song?> GetById(int id)
    {
        return await _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Album)
            .Include(s => s.Genres)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Song>> GetByArtistId(int artistId)
    {
        return await _context.Songs
            .Include(s => s.Album)
            .Include(s => s.Genres)
            .Where(s => s.ArtistId == artistId)
            .ToListAsync();
    }

    public async Task<List<Song>> GetByAlbumId(int albumId)
    {
        return await _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Genres)
            .Where(s => s.AlbumId == albumId)
            .ToListAsync();
    }

    public async Task<Song?> Create(Song song)
    {
        var artist = await _context.Artists.FindAsync(song.ArtistId);
        if (artist == null) return null;

        if (song.AlbumId.HasValue)
        {
            var album = await _context.Albums.FindAsync(song.AlbumId);
            if (album == null) return null;
        }

        _context.Songs.Add(song);
        await _context.SaveChangesAsync();
        
        return await _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Album)
            .Include(s => s.Genres)
            .FirstOrDefaultAsync(s => s.Id == song.Id);
    }

    public async Task<Song?> Update(int id, Song song)
    {
        var existing = await _context.Songs.FindAsync(id);
        if (existing == null) return null;

        var artist = await _context.Artists.FindAsync(song.ArtistId);
        if (artist == null) return null;

        if (song.AlbumId.HasValue)
        {
            var album = await _context.Albums.FindAsync(song.AlbumId);
            if (album == null) return null;
        }

        existing.Title = song.Title;
        existing.Duration = song.Duration;
        existing.ArtistId = song.ArtistId;
        existing.AlbumId = song.AlbumId;

        await _context.SaveChangesAsync();
        
        return await _context.Songs
            .Include(s => s.Artist)
            .Include(s => s.Album)
            .Include(s => s.Genres)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> Delete(int id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song == null) return false;

        _context.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddGenre(int songId, int genreId)
    {
        var song = await _context.Songs
            .Include(s => s.Genres)
            .FirstOrDefaultAsync(s => s.Id == songId);
        
        var genre = await _context.Genres.FindAsync(genreId);
        
        if (song == null || genre == null) return false;

        song.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveGenre(int songId, int genreId)
    {
        var song = await _context.Songs
            .Include(s => s.Genres)
            .FirstOrDefaultAsync(s => s.Id == songId);
        
        if (song == null) return false;

        var genre = song.Genres.FirstOrDefault(g => g.Id == genreId);
        if (genre == null) return false;

        if(song.Genres.Any(g=>g.Id == genreId))
        {
            song.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
