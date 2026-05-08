using FluentValidation;
using NetFoodia.Shared.ProfileDTOs;

namespace NetFoodia.Services.Validators.ProfileValidators
{
    public class UpsertVolunteerProfileValidator : AbstractValidator<UpsertVolunteerProfileDTO>
    {
        public UpsertVolunteerProfileValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(250);

            RuleFor(x => x.Location)
                .NotNull().WithMessage("Location is required.")
                .SetValidator(new GeoLocationValidator()!);

            RuleFor(x => x.VehicleType)
                .IsInEnum()
                .WithMessage($"VehicleType must be one of: " +
                             $"{string.Join(", ", Enum.GetNames(typeof(VehicleType)))}.");
        }
    }
}
