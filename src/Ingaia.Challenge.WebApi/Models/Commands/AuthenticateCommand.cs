using Ingaia.Challenge.WebApi.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ingaia.Challenge.WebApi.Models.Commands
{
    public class AuthenticateCommand
    {
        [Required(ErrorMessage = UserConstants.USERNAME_REQUIRED, AllowEmptyStrings = false)]        
        public string Username { get; set; }

        [Required(ErrorMessage = UserConstants.PASSWORD_REQUIRED, AllowEmptyStrings = false)]        
        public string Password { get; set; }
    }
}
