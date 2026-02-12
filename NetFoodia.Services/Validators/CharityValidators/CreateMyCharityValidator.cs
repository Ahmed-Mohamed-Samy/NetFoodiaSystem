using FluentValidation;
using NetFoodia.Shared.CharityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Validators.CharityValidators
{
    public class CreateMyCharityValidator : AbstractValidator<CreateMyCharityDTO>
    {
        public CreateMyCharityValidator()
        {
            RuleFor(x => x.OrganizationName)
                .RequiredField(nameof(CreateMyCharityDTO.OrganizationName))
                .LengthBetweenField(nameof(CreateMyCharityDTO.OrganizationName), 3, 200);

            RuleFor(x => x.LicenseNumber)
                .RequiredField(nameof(CreateMyCharityDTO.LicenseNumber))
                .LengthBetweenField(nameof(CreateMyCharityDTO.LicenseNumber), 3, 100);
        }
    }
}
