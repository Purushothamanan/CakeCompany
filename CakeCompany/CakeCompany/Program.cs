// See https://aka.ms/new-console-template for more information

using CakeCompany.Provider;

var shipmentProvider = new ShipmentProvider();
shipmentProvider.GetShipment();

Console.WriteLine("Shipment Details...");
