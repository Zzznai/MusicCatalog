namespace MusicCatalog.Api.DTOs.Requests;

public class CreateAwardRequest
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
}

public class UpdateAwardRequest
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
}
