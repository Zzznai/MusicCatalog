namespace MusicCatalog.Api.DTOs.Requests;

public class CreateAlbumRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ArtistId { get; set; }
}

public class UpdateAlbumRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ArtistId { get; set; }
}
