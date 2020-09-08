using FluentValidation;
using Ingaia.Challenge.WebApi.Constants;
using Ingaia.Challenge.WebApi.Entities;

namespace Ingaia.Challenge.WebApi.Validations
{
    public class CityRequestValidation : AbstractValidator<CityRequestEntity>
    {
        public CityRequestValidation()
        {
            RuleFor(x => x.CityName)
                .NotEmpty()
                    .WithMessage(CityRequestConstants.CITY_NAME_REQUIRED)
                .Length(3, 60)
                    .WithMessage(CityRequestConstants.CITY_NAME_LENGTH);
        }
    }
}
