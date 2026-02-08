using FluentValidation;
using MusicCatalog.Api.DTOs.Requests;

namespace MusicCatalog.Api.Validators;

public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
{
    public CreateGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required")
            .MaximumLength(100).WithMessage("Genre name cannot exceed 100 characters");
    }
}

public class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
{
    public UpdateGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required")
            .MaximumLength(100).WithMessage("Genre name cannot exceed 100 characters");
    }
}
