using FluentValidation;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.Validators.DonationValidators
{
    public class CreateDonationValidator : AbstractValidator<CreateDonationDTO>
    {
        public CreateDonationValidator()
        {
            // FoodType is now a strongly-typed enum — FluentValidation validates it is a
            // defined enum value automatically when model binding occurs. We add an explicit
            // IsInEnum() check here for clarity and to produce a friendly error message.
            RuleFor(x => x.FoodType)
                .IsInEnum()
                .WithMessage($"{nameof(CreateDonationDTO.FoodType)} must be a valid food category. " +
                             $"Allowed values: {string.Join(", ", Enum.GetValues<FoodType>().Select(v => $"{(int)v} ({v})"))}.");

            // UnitType is optional — when supplied it must be a valid enum member.
            RuleFor(x => x.UnitType)
                .IsInEnum()
                .When(x => x.UnitType.HasValue)
                .WithMessage($"{nameof(CreateDonationDTO.UnitType)} must be Kilos (1) or Meals (2).");

            RuleFor(x => x.Quantity)
                .RequiredNumberField(nameof(CreateDonationDTO.Quantity));

            RuleFor(x => x.PreparedTime)
                .Must(x => x <= DateTime.UtcNow.AddMinutes(5))
                .WithMessage($"{nameof(CreateDonationDTO.PreparedTime)} cannot be in the far future.");

            // ExpirationTime is no longer a client field — it is derived server-side.
            // All shelf-life logic lives in FoodExpiryPolicy.CalculateExpiry().

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage($"{nameof(CreateDonationDTO.Latitude)} must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage($"{nameof(CreateDonationDTO.Longitude)} must be between -180 and 180.");
        }
    }
}