using NUnit.Framework;
using CakeCompany.Provider;
using Moq;
using CakeCompany.Models;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CakeCompany.Models.Transport;

namespace CakeCompany.UnitTest
{

    [TestFixture]
    public class OrderServiceTests
    {

        ShipmentProvider _shipmentProvider;       
        //Mock Declaration
        private readonly Mock<ITransportProvider> _TransMock = new Mock<ITransportProvider>();
        private readonly Mock<IOrderProvider> _orderMock = new Mock<IOrderProvider>();
        private readonly Mock<ICakeProvider> _cakeMock = new Mock<ICakeProvider>();
        private readonly Mock<IPaymentProvider> _paymentMock = new Mock<IPaymentProvider>();
        private readonly Mock<ITransport> _transport = new Mock<ITransport>();

        // Initialize shipment provider with Mock object in construct 
        public OrderServiceTests()
        {
            var mocklogger = new Mock<ILogger<ShipmentProvider>>();
            _shipmentProvider = new ShipmentProvider(mocklogger.Object, _TransMock.Object, _orderMock.Object, _cakeMock.Object, _paymentMock.Object, _transport.Object);
        }

        [Test]
        public void GetLatestOrdersTest()
        {
            //Arrange
            var order = new List<Order>();
            order = CreateOrders();

            _orderMock.Setup(s => s.GetLatestOrders()).Returns(
                 new Order[]
                 {
                    new("ImportantCakeShop", DateTime.Now, 1, Cake.RedVelvet, 120.25)
                 }
            );           

            _cakeMock.Setup(s => s.Check(order[0])).Returns(DateTime.Now.Add(TimeSpan.FromMinutes(60)));

            _cakeMock.Setup(s => s.Bake(order[0])).Returns(
                new Product()
                {
                    Cake = Cake.RedVelvet,
                    Id = new Guid(),
                    Quantity = order[0].Quantity
                }
                );

             _paymentMock.Setup(P => P.Process(order[0])).Returns(
                    new PaymentIn()
                    {
                        HasCreditLimit = false,
                        IsSuccessful = true
                    }
                    );
                // Act
            _shipmentProvider.GetShipment();
        }

        [Test]
        public void ValidateOrdersTest_TrueFlag()
        {
            bool flag;
            //Arrange
            var order = new List<Order>();
            order = CreateOrders();

            _cakeMock.Setup(s => s.Check(order[0])).Returns(DateTime.Now.Add(TimeSpan.FromMinutes(60)));

            _cakeMock.Setup(s => s.Bake(order[0])).Returns(
                    new Product()
                    {
                        Cake = Cake.RedVelvet,
                        Id = new Guid(),
                        Quantity = order[0].Quantity
                    }
                );

            _paymentMock.Setup(P => P.Process(order[0])).Returns(
                    new PaymentIn()
                    {
                        HasCreditLimit = false,
                        IsSuccessful = true
                    }
                    );
            //Act
            flag = _shipmentProvider.ValidateOrders(order[0]);

            // Assert
            Assert.IsTrue(flag);
        }

        [Test]
        public void ValidateOrdersTest_FalseFlag()
        {
            bool flag;
            //Arrange
            var order = new List<Order>();
            order = CreateOrders();

            _cakeMock.Setup(s => s.Check(order[0])).Returns(DateTime.Now.Add(TimeSpan.FromMinutes(-60)));

            _cakeMock.Setup(s => s.Bake(order[0])).Returns(
                    new Product()
                    {
                        Cake = Cake.RedVelvet,
                        Id = new Guid(),
                        Quantity = order[0].Quantity
                    }
                );

            _paymentMock.Setup(P => P.Process(order[0])).Returns(
                    new PaymentIn()
                    {
                        HasCreditLimit = false,
                        IsSuccessful = true
                    }
                    );
            //Act
            flag = _shipmentProvider.ValidateOrders(order[0]);

            // Assert
            Assert.IsFalse(flag);
        }

        [Test]
        public void ShipmentModesTest()
        {
            //Arrange
            TransportProvider t = new TransportProvider();
            var order = new List<Order>();
            order = CreateOrders();
            var products = CreateProducts(order);

            //Act
            var transport = t.CheckForAvailability(products);

            // Assert     
            Assert.AreSame("Van", transport);
        }

        [Test]
        public void ShipmentModesTestwithDefault()
        {
            //Arrange
            TransportProvider t = new TransportProvider();
            
            List<Product> products = new List<Product>();

            //Act
            var transport = t.CheckForAvailability(products);

            // Assert
            Assert.AreSame("Ship", transport);
        }

        [Test]
        public void CheckTest()
        {
            //Arrange
            CakeProvider CP = new CakeProvider();
            var order = new List<Order>();
            order = CreateOrders();
            var products = CreateProducts(order);

            //Act
            var _datatime = CP.Check(order[0]);

            // Assert     
             Assert.AreNotEqual(DateTime.Now.Add(TimeSpan.FromMinutes(60)), _datatime);
            
        }

        [Test]
        public void CakeProviderBakeTest()
        {
            //Arrange
            CakeProvider CPCake = new CakeProvider();
            var order = new List<Order>();
            order = CreateOrders();
            var products = CreateProducts(order);

            //Act
            var _product = CPCake.Bake(order[0]);

            // Assert     
            Assert.AreEqual("RedVelvet", _product.Cake.ToString());
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", _product.Id.ToString());
            Assert.AreEqual(120.25, _product.Quantity);

        }
        #region utility method

        private List<Order> CreateOrders()
        {
            //Arrange
            List<Order> _Orders = new List<Order>()
           {       new("CakeBox", DateTime.Now, 1, Cake.RedVelvet, 120.25),
                   new("ImportantCakeShop", DateTime.Now, 1, Cake.RedVelvet, 120.25)
           };
            return _Orders;
        }
        private List<Product> CreateProducts(List<Order> order)
        {
            //Arrange
            List<Product> products = new List<Product>()
            {   new Product {
                    Cake = Cake.RedVelvet,
                    Id = new Guid(),
                    Quantity = order[0].Quantity
                },
                 new Product {
                    Cake = Cake.Chocolate,
                    Id = new Guid(),
                    Quantity = order[1].Quantity
                }
            };

            return  products;
        }
        #endregion
    }
}
