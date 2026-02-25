using FluentValidation;
using NetFoodia.Shared.ProfileDTOs;

namespace NetFoodia.Services.Validators.ProfileValidators
{
    public class UpsertDonorProfileValidator : AbstractValidator<UpsertDonorProfileDTO>
    {
        public UpsertDonorProfileValidator()
        {
            RuleFor(d => d.Address)
                .RequiredField(nameof(UpsertDonorProfileDTO.Address))
                .MaxLengthField(nameof(UpsertDonorProfileDTO.Address), 250);

            When(x => x.IsBusiness, () =>
            {
                RuleFor(x => x.BusinessType)
                    .NotEmpty().WithMessage("BusinessType is required when IsBusiness is true.")
                    .MaximumLength(100);

                RuleFor(x => x.Location)
                    .NotNull().WithMessage("Location is required when IsBusiness is true.");

                RuleFor(x => x.Address)
                    .NotEmpty().WithMessage("Address is required when IsBusiness is true.");
            });

            When(x => !x.IsBusiness, () =>
            {
                RuleFor(x => x.BusinessType)
                    .Must(string.IsNullOrWhiteSpace)
                    .WithMessage("BusinessType must be empty when IsBusiness is false.");
            });

            RuleFor(x => x.Location)
                .SetValidator(new GeoLocationValidator()!)
                .When(x => x.Location is not null);
        }
    }
}
