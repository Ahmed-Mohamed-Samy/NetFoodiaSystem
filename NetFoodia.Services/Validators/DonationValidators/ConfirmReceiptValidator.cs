using FluentValidation;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.Validators.DonationValidators
{
    public class ConfirmReceiptValidator : AbstractValidator<ConfirmReceiptDTO>
    {
        public ConfirmReceiptValidator()
        {
            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters");
        }
    }
}
