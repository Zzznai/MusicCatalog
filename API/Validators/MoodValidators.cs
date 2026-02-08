using FluentValidation;
using MusicCatalog.Api.DTOs.Requests;

namespace MusicCatalog.Api.Validators;

public class CreateMoodRequestValidator : AbstractValidator<CreateMoodRequest>
{
    public CreateMoodRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Mood name is required")
            .MaximumLength(50).WithMessage("Mood name cannot exceed 50 characters");
    }
}

public class UpdateMoodRequestValidator : AbstractValidator<UpdateMoodRequest>
{
    public UpdateMoodRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Mood name is required")
            .MaximumLength(50).WithMessage("Mood name cannot exceed 50 characters");
    }
}
