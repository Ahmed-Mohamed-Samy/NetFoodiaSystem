using FluentValidation;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.Validators.DonationValidators
{
    public class EditDonationValidator : AbstractValidator<EditDonationDTO>
    {
        public EditDonationValidator()
        {
            RuleFor(x => x.FoodType)
                .IsInEnum()
                .WithMessage($"{nameof(EditDonationDTO.FoodType)} must be a valid food category. " +
                             $"Allowed values: {string.Join(", ", Enum.GetValues<FoodType>().Select(v => $"{(int)v} ({v})"))}.");

            RuleFor(x => x.UnitType)
                .IsInEnum()
                .When(x => x.UnitType.HasValue)
                .WithMessage($"{nameof(EditDonationDTO.UnitType)} must be Kilos (1) or Meals (2).");

            RuleFor(x => x.Quantity)
                .RequiredNumberField(nameof(EditDonationDTO.Quantity));

            RuleFor(x => x.PreparedTime)
                .Must(x => x <= DateTime.UtcNow.AddMinutes(5))
                .WithMessage($"{nameof(EditDonationDTO.PreparedTime)} cannot be in the far future.");

            // ExpirationTime is re-derived from FoodType + PreparedTime on every edit.

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage($"{nameof(EditDonationDTO.Latitude)} must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage($"{nameof(EditDonationDTO.Longitude)} must be between -180 and 180.");
        }
    }
}
