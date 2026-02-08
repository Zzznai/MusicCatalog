using FluentValidation;
using MusicCatalog.Api.DTOs.Requests;

namespace MusicCatalog.Api.Validators;

public class CreateAwardRequestValidator : AbstractValidator<CreateAwardRequest>
{
    public CreateAwardRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Award name is required")
            .MaximumLength(100).WithMessage("Award name cannot exceed 100 characters");

        RuleFor(x => x.Year)
            .GreaterThan(1900).WithMessage("Year must be after 1900")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future");
    }
}

public class UpdateAwardRequestValidator : AbstractValidator<UpdateAwardRequest>
{
    public UpdateAwardRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Award name is required")
            .MaximumLength(100).WithMessage("Award name cannot exceed 100 characters");

        RuleFor(x => x.Year)
            .GreaterThan(1900).WithMessage("Year must be after 1900")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future");
    }
}
