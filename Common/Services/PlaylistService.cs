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
            .ToListAsync();
    }

    public async Task<Playlist?> GetById(int id)
    {
        return await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Playlist>> GetByUserId(int userId)
    {
        return await _context.Playlists
            .Include(p => p.Songs)
            .Where(p => p.UserId == userId)
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
            .FirstOrDefaultAsync(p => p.Id == playlist.Id);
    }

    public async Task<Playlist?> Update(int id, Playlist playlist)
    {
        var existing = await _context.Playlists.FindAsync(id);
        if (existing == null) return null;

        var user = await _context.Users.FindAsync(playlist.UserId);
        if (user == null) return null;

        existing.Name = playlist.Name;
        existing.UserId = playlist.UserId;

        await _context.SaveChangesAsync();
        
        return await _context.Playlists
            .Include(p => p.User)
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> Delete(int id)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null) return false;

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddSong(int playlistId, int songId)
    {
        var playlist = await _context.Playlists
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == playlistId);
        
        var song = await _context.Songs.FindAsync(songId);
        
        if (playlist == null || song == null) return false;

        playlist.Songs.Add(song);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveSong(int playlistId, int songId)
    {
        var playlist = await _context.Playlists
            .Include(p => p.Songs)
            .FirstOrDefaultAsync(p => p.Id == playlistId);
        
        if (playlist == null) return false;

        var song = playlist.Songs.FirstOrDefault(s => s.Id == songId);
        if (song == null) return false;

        playlist.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }
}
