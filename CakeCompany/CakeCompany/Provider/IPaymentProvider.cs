using CakeCompany.Models;

namespace CakeCompany.Provider
{
    public interface IPaymentProvider
    {
        public PaymentIn Process(Order order);
    }
}
