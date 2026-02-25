using FluentValidation;
using NetFoodia.Shared.ProfileDTOs;

namespace NetFoodia.Services.Validators.ProfileValidators
{
    public class SetVolunteerStatusValidator : AbstractValidator<SetVolunteerStatusDTO>
    {
        public SetVolunteerStatusValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid status value.");
        }
    }
}
