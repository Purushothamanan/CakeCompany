// See https://aka.ms/new-console-template for more information

using CakeCompany.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CakeCompany.Models.Transport;

var serviceProvider = new ServiceCollection()
                            .AddLogging(builder => {
                                builder.ClearProviders();
                                builder.AddConsole();
                                builder.SetMinimumLevel(LogLevel.Information);

                            })
                            .AddSingleton<ITransport, Ship>()
                            .AddSingleton<ITransport, Truck>()
                            .AddSingleton<ITransport, Van>()
                            .AddSingleton<ITransportProvider, TransportProvider>()
                            .AddSingleton<IOrderProvider, OrderProvider>()
                            .AddSingleton<ICakeProvider, CakeProvider>()
                            .AddSingleton<IPaymentProvider, PaymentProvider>()
                            .BuildServiceProvider();

var logger = serviceProvider.GetService<ILogger<ShipmentProvider>>();
var Trans = serviceProvider.GetService<ITransportProvider>();
var order = serviceProvider.GetService<IOrderProvider>();
var cake = serviceProvider.GetService<ICakeProvider>();
var payment = serviceProvider.GetService<IPaymentProvider>();
var Transport = serviceProvider.GetService<ITransport>();
Console.WriteLine("Shipment Details...");
var shipmentProvider = new ShipmentProvider(logger,Trans,order,cake,payment,Transport);

shipmentProvider.GetShipment();

