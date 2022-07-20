
namespace CakeCompany.Models.Transport
{
    public interface ITransport
    {
        public bool Deliver(List<Product> products);
    }
}
