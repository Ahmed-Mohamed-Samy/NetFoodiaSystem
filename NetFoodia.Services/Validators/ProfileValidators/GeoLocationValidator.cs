using FluentValidation;
using NetFoodia.Shared.ProfileDTOs;

namespace NetFoodia.Services.Validators.ProfileValidators
{
    public class GeoLocationValidator : AbstractValidator<GeoLocationDTO>
    {
        public GeoLocationValidator()
        {

            RuleFor(x => x.Latitude)
                .NotEmpty()
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .NotEmpty()
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180.");
        }
    }
}
