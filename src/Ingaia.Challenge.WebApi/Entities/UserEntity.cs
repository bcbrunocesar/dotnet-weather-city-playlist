using FluentValidation.Results;
using Ingaia.Challenge.WebApi.Validations;

namespace Ingaia.Challenge.WebApi.Entities
{
    public class UserEntity
    {
        public UserEntity(string fullname, string username)
        {
            Fullname = fullname;
            Username = username;
        }

        public int Id { get; private set; }
        public string Fullname { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Role { get; } = "REGULAR";
        public ValidationResult ValidationResult { get; set; }

        public void SetPasswordHashed(string hashedPassword) => Password = hashedPassword;

        public bool IsValid()
        {
            ValidationResult = new UserValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
