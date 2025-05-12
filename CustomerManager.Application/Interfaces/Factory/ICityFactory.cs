using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Interfaces.Factory
{
    public interface ICityFactory
    {
        object CreateCity(string cityName, string countryName); 
    }
}
