using CakeCompany.Models;

namespace CakeCompany.Provider
{
    public interface ICakeProvider
    {
        public DateTime Check(Order order);
        public Product Bake(Order order);
    }
}
