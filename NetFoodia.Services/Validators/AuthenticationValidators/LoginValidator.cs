using FluentValidation;
using NetFoodia.Shared.AuthenticationDTOs;

namespace NetFoodia.Services.Validators.AuthenticationValidators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(l => l.Email).RequiredField(nameof(LoginDTO.Email));

            RuleFor(l => l.Password).RequiredField(nameof(LoginDTO.Password));
        }
    }
}
