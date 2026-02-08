namespace MusicCatalog.Api.DTOs.Requests;

public class CreateRecordLabelRequest
{
    public string Name { get; set; } = string.Empty;
    public string BasedIn { get; set; } = string.Empty;
    public int FoundedYear { get; set; }
}

public class UpdateRecordLabelRequest
{
    public string Name { get; set; } = string.Empty;
    public string BasedIn { get; set; } = string.Empty;
    public int FoundedYear { get; set; }
}
