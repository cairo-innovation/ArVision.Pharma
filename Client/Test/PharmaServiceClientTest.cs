
using NUnit.Framework;
namespace ArVision.Service.Client.Test
{
    [TestFixture]
    public class PharmaServiceClientTest
    {
        [Test]
        public void Create()
        {
            PharmaServiceProxy client = new PharmaServiceFactory().GetPharmaServiceProxy("127.0.0.1");
            Assert.IsNotNull(client);
        }
    }
}
