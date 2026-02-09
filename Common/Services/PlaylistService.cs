using Microsoft.EntityFrameworkCore;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Persistance;

namespace MusicCatalog.Common.Services;

public class PlaylistService : BaseService
{
    public PlaylistService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Playlist>> GetAll()
    {
        return await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.Songs)
            .ThenInclude(s=>s.Artist)
            .ToListAsync();
    }

    public async Task<Playlist?> GetById(int id)
    {
        return await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.Songs)
            .ThenInclude(s=>s.Artist)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Playlist>> GetByUserId(int userId)
    {
        return await _context.Playlists
            .Include(p => p.Songs)
            .ThenInclude(s=>s.Artist)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Playlist>> GetByUsername(string username)
    {
        return await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.Songs)
            .ThenInclude(s => s.Artist)
            .Where(p => p.User.Username == username)
            .ToListAsync();
    }

    public async Task<Playlist?> Create(Playlist playlist)
    {
        var user = await _context.Users.FindAsync(playlist.UserId);
        if (user == null) return null;

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();
        
        return await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.Songs)
            .ThenInclude(s=>s.Artist)
            .FirstOrDefaultAsync(p => p.Id == playlist.Id);
    }

    public async Task<Playlist?> Update(int id, int userId, string name)
    {
        var existing = await _context.Playlists.FindAsync(id);
        if (existing == null) return null;

        if (existing.UserId != userId) return null;

        existing.Name = name;

        await _context.SaveChangesAsync();
        
        return await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.Songs)
            .ThenInclude(s=>s.Artist)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> Delete(int id, int userId)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null) return false;

        if (playlist.UserId != userId) return false;

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddSong(int playlistId, int userId, int songId)
    {
        var playlist = await _context.Playlists
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == playlistId);
        
        if (playlist == null) return false;

        if (playlist.UserId != userId) return false;

        if (playlist.Songs.Any(s => s.Id == songId)) return false;

        var song = await _context.Songs.FindAsync(songId);
        if (song == null) return false;

        playlist.Songs.Add(song);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveSong(int playlistId, int userId, int songId)
    {
        var playlist = await _context.Playlists
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == playlistId);
        
        if (playlist == null) return false;

        if (playlist.UserId != userId) return false;

        var song = playlist.Songs.FirstOrDefault(s => s.Id == songId);
        if (song == null) return false;

        playlist.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }
}
