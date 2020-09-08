using Ingaia.Challenge.WebApi.Constants;
using System.ComponentModel.DataAnnotations;

namespace Ingaia.Challenge.WebApi.Models.Commands
{
    public class AddCityRequestCommand
    {
        public AddCityRequestCommand(string cityName)
        {
            CityName = cityName;
        }

        [Required(ErrorMessage = CityRequestConstants.CITY_NAME_REQUIRED, AllowEmptyStrings = false)]
        [StringLength(40, ErrorMessage = CityRequestConstants.CITY_NAME_LENGTH, MinimumLength = 10)]
        public string CityName { get; set; }
    }
}
