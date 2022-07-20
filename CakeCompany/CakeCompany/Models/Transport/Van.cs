namespace CakeCompany.Models.Transport;

internal class Van : ITransport
{
    public bool Deliver(List<Product> products)
    {
        return true;
    }
}