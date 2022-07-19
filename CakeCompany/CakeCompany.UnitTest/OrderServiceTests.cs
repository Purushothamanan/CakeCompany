using NUnit.Framework;
using CakeCompany.Provider;
using Moq;

namespace CakeCompany.UnitTest
{
    public class  OrderServiceTests
    {

        private readonly OrderProvider _ord;

        private readonly Mock<IOrderProvider> _OrderProviderMock = new Mock<IOrderProvider>();
       // private readonly Mock<ILoggingService> _loggerMock = new Mock<ILoggingService>();

        public OrderServiceTests ()
        {
            _ord = new OrderProvider (_OrderProviderMock);
        }

        [Test]
        public void GetLatestOrdersTest()
        {
           

        }


    }
}
