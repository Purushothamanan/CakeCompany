using System.Diagnostics;
using CakeCompany.Models;
using CakeCompany.Models.Transport;

namespace CakeCompany.Provider;

internal class ShipmentProvider
{
    public void GetShipment()
    {
        //Call order to get new orders
        var orderProvider = new OrderProvider();

        var orders = orderProvider.GetLatestOrders();

        if (!orders.Any())
        {
            return;
        }

        var cancelledOrders = new List<Order>();
        var products = new List<Product>();

        foreach (var order in orders)
        {
            var cakeProvider = new CakeProvider();

            var estimatedBakeTime = cakeProvider.Check(order);

            if (estimatedBakeTime > order.EstimatedDeliveryTime)
            {
                cancelledOrders.Add(order);
                continue;
            }

            var payment = new PaymentProvider();

            if (!payment.Process(order).IsSuccessful)
            {
                cancelledOrders.Add(order);
                continue;
            }

            var product = cakeProvider.Bake(order);
            products.Add(product);
        }
            
        var transportProvider = new TransportProvider();

        var transport = transportProvider.CheckForAvailability(products);

        if (transport == "Van")
        {
            var van = new Van();
            van.Deliver(products);
        }

        if (transport == "Truck")
        {
            var truck = new Truck();
            truck.Deliver(products);
        }

        if (transport == "Ship")
        {
            var ship = new Ship();
            ship.Deliver(products);
        }
    }
}
