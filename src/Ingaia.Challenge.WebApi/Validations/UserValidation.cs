using FluentValidation;
using Ingaia.Challenge.WebApi.Constants;
using Ingaia.Challenge.WebApi.Entities;

namespace Ingaia.Challenge.WebApi.Validations
{
    public class UserValidation : AbstractValidator<UserEntity>
    {
        public UserValidation()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                    .WithMessage(UserConstants.USERNAME_REQUIRED)
                .Length(3, 12)
                    .WithMessage(UserConstants.USERNAME_LENGTH);

            //RuleFor(x => x.Password)
            //    .NotEmpty()
            //        .WithMessage(UserConstants.PASSWORD_REQUIRED)
            //    .Length(6, 12)
            //        .WithMessage(UserConstants.PASSWORD_LENGTH);

            RuleFor(x => x.Fullname)
                .NotEmpty()
                    .WithMessage(UserConstants.FULLNAME_REQUIRED)
                .Length(6, 40)
                    .WithMessage(UserConstants.FULLNAME_LENGTH);
        }
    }
}
