using FluentValidation;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.Validators.DonationValidators
{
    public class InspectDonationValidator : AbstractValidator<InspectDonationDTO>
    {
        public InspectDonationValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty()
                .When(x => !x.IsApproved)
                .WithMessage("Reason is required when rejecting a donation");
        }
    }
}
