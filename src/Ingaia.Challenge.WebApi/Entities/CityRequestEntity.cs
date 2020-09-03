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
    }
}
