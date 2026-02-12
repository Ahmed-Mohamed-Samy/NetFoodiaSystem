using FluentValidation;
using NetFoodia.Shared.CharityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Validators.CharityValidators
{
    public class UpdateCharityInfoValidator : AbstractValidator<UpdateCharityInfoDTO>
    {
        public UpdateCharityInfoValidator()
        {
            RuleFor(x => x.OrganizationName)
                .RequiredField(nameof(UpdateCharityInfoDTO.OrganizationName))
                .LengthBetweenField(nameof(UpdateCharityInfoDTO.OrganizationName), 3, 200);

            RuleFor(x => x.LicenseNumber)
                .RequiredField(nameof(UpdateCharityInfoDTO.LicenseNumber))
                .LengthBetweenField(nameof(UpdateCharityInfoDTO.LicenseNumber), 3, 100);
        }
    }
}
