using FluentValidation;
using MusicCatalog.Api.DTOs.Requests;

namespace MusicCatalog.Api.Validators;

public class CreateSongRequestValidator : AbstractValidator<CreateSongRequest>
{
    public CreateSongRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be greater than zero");

        RuleFor(x => x.ArtistId)
            .GreaterThan(0).WithMessage("ArtistId must be greater than 0");

        RuleFor(x => x.AlbumId)
            .GreaterThan(0).When(x => x.AlbumId.HasValue)
            .WithMessage("AlbumId must be greater than 0");
    }
}

public class UpdateSongRequestValidator : AbstractValidator<UpdateSongRequest>
{
    public UpdateSongRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be greater than zero");

        RuleFor(x => x.ArtistId)
            .GreaterThan(0).WithMessage("ArtistId must be greater than 0");

        RuleFor(x => x.AlbumId)
            .GreaterThan(0).When(x => x.AlbumId.HasValue)
            .WithMessage("AlbumId must be greater than 0");
    }
}
