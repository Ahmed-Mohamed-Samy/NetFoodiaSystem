using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using NetFoodia.Shared.DeliveryDTOs;

namespace NetFoodia.Services.Validators.DeliveryValidators
{
    public class AssignTaskValidator : AbstractValidator<AssignTaskDTO>
    {
        public AssignTaskValidator()
        {
            RuleFor(x => x.VolunteerUserId)
                .RequiredField(nameof(AssignTaskDTO.VolunteerUserId));
        }
    }
}
