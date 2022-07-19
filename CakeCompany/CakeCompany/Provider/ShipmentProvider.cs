using System.Diagnostics;
using CakeCompany.Models;
using CakeCompany.Models.Transport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CakeCompany.Provider;

public class ShipmentProvider
{
    private readonly ILogger<ShipmentProvider> _logger;
    private readonly ITransportProvider _transportProvider;
    private readonly IOrderProvider _orderProvider;
    private readonly ICakeProvider _cakeProvider;
    private readonly IPaymentProvider _paymentProvider;   

    public ShipmentProvider (ILogger<ShipmentProvider> log,
        ITransportProvider transportProvider,
        IOrderProvider orderProvider, 
        ICakeProvider cakeProvider, 
        IPaymentProvider paymentProvider)
    {
        _logger = log;    
        _transportProvider = transportProvider;
        _orderProvider = orderProvider;
        _cakeProvider = cakeProvider;
        _paymentProvider = paymentProvider;
    }
    public void GetShipment()
    {
        _logger.LogInformation("GetShipment Details..");

        //Call order to get new orders        
        var orders = _orderProvider.GetLatestOrders();

        try
        {
            //Will check for any order in place then true
            if (orders.Any())
            {
                var cancelledOrders = new List<Order>();
                var products = new List<Product>();                
                foreach (var order in orders)
                {                 
                    //New Method to Validate successfull payments and estimated time to deliver 
                    //based on that order will be cancelled
                    if (ValidateOrders(order))
                    {
                        cancelledOrders.Add(order);
                        continue;
                    }
                    var product = _cakeProvider.Bake(order);
                    products.Add(product);
                }
                var transport = _transportProvider.CheckForAvailability(products);
                //New Method to chose and deliver products
                ShipmentMode(transport, products);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("GetShipment Error" + ex .Message.ToString());
        }
    }

    ///*************************************************************************************
    /// <summary>
    /// ShipmentMode method to decided the mode of transport and to deliver the products
    /// </summary>
    /// <design_author>Purushothaman V</design_author>
    /// <changes></changes>
    /// <param name="transport"></param>
    /// <param name="products"></param>
    ///*************************************************************************************
    public void ShipmentMode(string transport,List<Product> products)
    {
        _logger.LogInformation("ShipmentMode Method Called..");

        switch (transport)
        {
            case "Van":
                var van = new Van();
                van.Deliver(products);
                break;
            case "Truck":
                var truck = new Truck();
                truck.Deliver(products);
                break;
            case "Ship":
                var ship = new Ship();
                ship.Deliver(products);
                break;
        }
    }

    ///*************************************************************************************
    /// <summary>
    /// ValidateOrders method to vaidate successfull payment and estimated time to deliver
    /// </summary>
    /// <design_author>Purushothaman V</design_author>
    /// <changes></changes>
    /// <param name="order"></param>    
    ///*************************************************************************************
    public bool ValidateOrders(Order order)
    {
        _logger.LogInformation("ValidateOrders Method Called..");
       
        var estimatedBakeTime = _cakeProvider.Check(order);        
        
        if((!_paymentProvider.Process(order).IsSuccessful) || (estimatedBakeTime > order.EstimatedDeliveryTime))
        {
            return true;
        }

        return false;
    }
}
