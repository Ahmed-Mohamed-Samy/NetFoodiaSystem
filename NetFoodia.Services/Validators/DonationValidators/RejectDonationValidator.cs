using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.Validators.DonationValidators
{
    public class RejectDonationValidator : AbstractValidator<RejectDonationDTO>
    {
        public RejectDonationValidator()
        {
            RuleFor(x => x.Reason)
                .RequiredField(nameof(RejectDonationDTO.Reason))
                .LengthBetweenField(nameof(RejectDonationDTO.Reason), 3, 500);
        }
    }
}
