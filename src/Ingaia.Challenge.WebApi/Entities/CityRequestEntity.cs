using FluentValidation.Results;
using Ingaia.Challenge.WebApi.Validations;
using System;

namespace Ingaia.Challenge.WebApi.Entities
{
    public class CityRequestEntity
    {
        public CityRequestEntity(string cityName)
        {
            CityName = cityName;
            RequestDate = DateTime.Now;
        }

        public int Id { get; private set; }
        public string CityName { get; private set; }
        public DateTime RequestDate { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            ValidationResult = new CityRequestValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
