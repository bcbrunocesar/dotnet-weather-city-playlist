using Ingaia.Challenge.WebApi.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ingaia.Challenge.WebApi.Models.Commands
{
    public class RegisterUserCommand
    {
        [Required(ErrorMessage = UserConstants.FULLNAME_REQUIRED, AllowEmptyStrings = false)]
        [StringLength(40, ErrorMessage = UserConstants.FULLNAME_LENGTH, MinimumLength = 10)]
        public string Fullname { get; set; }

        [Required(ErrorMessage = UserConstants.USERNAME_REQUIRED, AllowEmptyStrings = false)]
        [StringLength(12, MinimumLength = 3, ErrorMessage = UserConstants.USERNAME_LENGTH)]
        public string Username { get; set; }

        [Required(ErrorMessage = UserConstants.PASSWORD_REQUIRED, AllowEmptyStrings = false)]
        [StringLength(12, MinimumLength = 6, ErrorMessage = UserConstants.PASSWORD_LENGTH)]
        public string Password { get; set; }
    }
}
