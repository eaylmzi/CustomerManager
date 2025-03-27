using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Domain.ReadModels
{
    public class CustomerWithCity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get;  set; }
        [Column("surname")]
        public string Surname { get;  set; }
        [Column("balance")]
        public decimal Balance { get;  set; }
        [Column("status")]
        public bool Status { get;  set; }

        [Column("city_name")]
        public string CityName { get;  set; }
        [Column("city_country")]
        public string CityCountry { get;  set; }
    }
}
