using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Domain.Entities
{
    public class City
    {
        [Column("city_name")]
        public string CityName { get; private set; }
        [Column("city_country")]
        public string CityCountry { get; private set; }

        public City()
        {

        }
        public City(string cityName, string cityCountry)
        {
            CityName = cityName;
            CityCountry = cityCountry;
        }
    }
}
