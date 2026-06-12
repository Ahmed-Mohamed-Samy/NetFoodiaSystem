using System.ComponentModel.DataAnnotations;

namespace NetFoodia.Shared.AuthenticationDTOs
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "OTP is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits.")]
        public string OTP { get; set; } = null!;

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Confirm new password is required.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
