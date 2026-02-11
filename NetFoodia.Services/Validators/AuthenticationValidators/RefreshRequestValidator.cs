using FluentValidation;
using NetFoodia.Shared.AuthenticationDTOs;

namespace NetFoodia.Services.Validators.AuthenticationValidators
{
    public class RefreshRequestValidator : AbstractValidator<RefreshRequestDTO>
    {
        public RefreshRequestValidator()
        {
            RuleFor(r => r.RefreshToken).RequiredField(nameof(RefreshRequestDTO.RefreshToken));
        }
    }
}
