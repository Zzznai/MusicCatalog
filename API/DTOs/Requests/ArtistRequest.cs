namespace MusicCatalog.Api.DTOs.Requests;

public class CreateArtistRequest
{
    public string StageName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RecordLabelId { get; set; }
}

public class UpdateArtistRequest
{
    public string StageName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RecordLabelId { get; set; }
}
