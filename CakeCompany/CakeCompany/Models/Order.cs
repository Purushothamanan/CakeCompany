namespace CakeCompany.Models;

internal record Order(string ClientName, DateTime EstimatedDeliveryTime, int Id, Cake Name, double Quantity);