using FluentValidation;
using NetFoodia.Shared.AuthenticationDTOs;

namespace NetFoodia.Services.Validators.AuthenticationValidators
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(r => r.FullName)
                .RequiredField(nameof(RegisterDTO.FullName))
                .LengthBetweenField(nameof(RegisterDTO.FullName), 2, 50)
                .LettersOnlyField(nameof(RegisterDTO.FullName));

            RuleFor(r => r.Phone)
                .RequiredField(nameof(RegisterDTO.Phone))
                .PhoneField(nameof(RegisterDTO.Phone));


            RuleFor(r => r.Password)
                .RequiredField(nameof(RegisterDTO.Password))
                .MinLengthField(nameof(RegisterDTO.Password), 8)
                .PasswordField(nameof(RegisterDTO.Password));

            RuleFor(r => r.Role)
                .EnumField(nameof(RegisterDTO.Role));
        }
    }
}
