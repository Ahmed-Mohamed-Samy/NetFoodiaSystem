using FluentValidation;
using NetFoodia.Shared.CharityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Validators.CharityValidators
{
    public class UpdateCharityLocationValidator : AbstractValidator<UpdateCharityLocationDTO>
    
    {
        public UpdateCharityLocationValidator()
        {
            When(x => x.Latitude.HasValue, () =>
            {
                RuleFor(x => x.Latitude!.Value).InclusiveBetween(-90, 90);
            });

            When(x => x.Longitude.HasValue, () =>
            {
                RuleFor(x => x.Longitude!.Value).InclusiveBetween(-180, 180);
            });
        }
    }
}
