using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Dtos.Customers
{
    public class CustomerDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public decimal Balance { get; set; }
        public bool Status { get; set; }
        public string CityName { get; set; } = null!;
        public string CityCountry { get; set; } = null!;
    }
}
