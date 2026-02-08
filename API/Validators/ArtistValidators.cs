using FluentValidation;
using MusicCatalog.Api.DTOs.Requests;

namespace MusicCatalog.Api.Validators;

public class CreateArtistRequestValidator : AbstractValidator<CreateArtistRequest>
{
    public CreateArtistRequestValidator()
    {
        RuleFor(x => x.StageName)
            .NotEmpty().WithMessage("Stage name is required")
            .MaximumLength(100).WithMessage("Stage name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.RecordLabelId)
            .GreaterThan(0).WithMessage("Record label is required");
    }
}

public class UpdateArtistRequestValidator : AbstractValidator<UpdateArtistRequest>
{
    public UpdateArtistRequestValidator()
    {
        RuleFor(x => x.StageName)
            .NotEmpty().WithMessage("Stage name is required")
            .MaximumLength(100).WithMessage("Stage name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.RecordLabelId)
            .GreaterThan(0).WithMessage("Record label is required");
    }
}
