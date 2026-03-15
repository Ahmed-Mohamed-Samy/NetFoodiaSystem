using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.Validators.DonationValidators
{
    public class CreateDonationValidator : AbstractValidator<CreateDonationDTO>
    {
        public CreateDonationValidator()
        {
            RuleFor(x => x.FoodType)
                .RequiredField(nameof(CreateDonationDTO.FoodType))
                .LengthBetweenField(nameof(CreateDonationDTO.FoodType), 3, 200);

            RuleFor(x => x.Quantity)
                .RequiredNumberField(nameof(CreateDonationDTO.Quantity));

            RuleFor(x => x.PreparedTime)
                .Must(x => x <= DateTime.UtcNow.AddMinutes(5))
                .WithMessage($"{nameof(CreateDonationDTO.PreparedTime)} can not be in the far future");

            RuleFor(x => x.ExpirationTime)
                .Must(x => x > DateTime.UtcNow)
                .WithMessage($"{nameof(CreateDonationDTO.ExpirationTime)} must be in the future");

            RuleFor(x => x)
                .Must(x => x.ExpirationTime > x.PreparedTime)
                .WithMessage($"{nameof(CreateDonationDTO.ExpirationTime)} must be after {nameof(CreateDonationDTO.PreparedTime)}");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage($"{nameof(CreateDonationDTO.Latitude)} must be between -90 and 90");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage($"{nameof(CreateDonationDTO.Longitude)} must be between -180 and 180");
        }
    }
}