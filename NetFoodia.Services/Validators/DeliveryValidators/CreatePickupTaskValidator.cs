using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using NetFoodia.Shared.DeliveryDTOs;

namespace NetFoodia.Services.Validators.DeliveryValidators
{
    public class CreatePickupTaskValidator : AbstractValidator<CreatePickupTaskDTO>
    {
        public CreatePickupTaskValidator()
        {
            RuleFor(x => x.SlaDueAt)
                .Must(x => !x.HasValue || x.Value > DateTime.UtcNow)
                .WithMessage($"{nameof(CreatePickupTaskDTO.SlaDueAt)} must be in the future");
        }
    }
}
