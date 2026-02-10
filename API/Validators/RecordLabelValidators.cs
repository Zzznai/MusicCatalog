using FluentValidation;
using MusicCatalog.Api.DTOs.Requests;

namespace MusicCatalog.Api.Validators;

public class CreateRecordLabelRequestValidator : AbstractValidator<CreateRecordLabelRequest>
{
    public CreateRecordLabelRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Record label name is required")
            .MaximumLength(100).WithMessage("Record label name cannot exceed 100 characters");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("CountryId is required");

        RuleFor(x => x.FoundedYear)
            .InclusiveBetween(1900, 2100).WithMessage("Founded year must be between 1900 and 2100");
    }
}

public class UpdateRecordLabelRequestValidator : AbstractValidator<UpdateRecordLabelRequest>
{
    public UpdateRecordLabelRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Record label name is required")
            .MaximumLength(100).WithMessage("Record label name cannot exceed 100 characters");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("CountryId is required");

        RuleFor(x => x.FoundedYear)
            .InclusiveBetween(1900, 2100).WithMessage("Founded year must be between 1900 and 2100");
    }
}
