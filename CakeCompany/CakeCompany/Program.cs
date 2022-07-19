// See https://aka.ms/new-console-template for more information

using CakeCompany.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
                            .AddLogging(builder => {
                                builder.ClearProviders();
                                builder.AddConsole();
                                builder.SetMinimumLevel(LogLevel.Information);

                            })
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

var shipmentProvider = new ShipmentProvider(logger, Trans, order, cake, payment);

shipmentProvider.GetShipment();

Console.WriteLine("Shipment Details...");