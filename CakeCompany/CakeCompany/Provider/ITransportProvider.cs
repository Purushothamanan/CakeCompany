using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeCompany.Models;

namespace CakeCompany.Provider
{
    public interface ITransportProvider
    {
        public string CheckForAvailability(List<Product> products);
    }
}
