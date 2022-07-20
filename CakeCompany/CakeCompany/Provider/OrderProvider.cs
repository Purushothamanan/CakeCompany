using System.Reflection.Metadata.Ecma335;
using CakeCompany.Models;

namespace CakeCompany.Provider;
public class OrderProvider : IOrderProvider
{
    public Order[] GetLatestOrders()
    {
        return new Order[]
        {
            new("CakeBox", DateTime.Now, 1, Cake.RedVelvet, 120.25),
            new("ImportantCakeShop", DateTime.Now, 1, Cake.RedVelvet, 120.25)
        };
    }

    public void UpdateOrders(Order[] orders)
    {
               
        
    }
}


