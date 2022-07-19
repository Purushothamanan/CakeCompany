namespace CakeCompany.Models;

public record Order(string ClientName, DateTime EstimatedDeliveryTime, int Id, Cake Name, double Quantity);