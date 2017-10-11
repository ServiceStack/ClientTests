using NUnit.Framework;
using ServiceStack;
using Test.ServiceInterface;

namespace ClientTest
{
    [TestFixture]
    class TestInterfaceMarkerTests
    {
        private string clientUrl = "http://test.servicestack.net";

        [Test]
        public void test_Does_SendDefault_as_POST()
        {
            var client = new JsonServiceClient(clientUrl);

            var request = new SendDefault{ Id = 1 };
            var response = client.Send(request);

            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Post));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendDefault"));
        }

        [Test]
        public void test_Does_SendRestGet_as_GET_using_Predefined_Route()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new SendRestGet() { Id = 1 };

            var response = client.Send(request);
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Get));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendRestGet"));
        }

        [Test]
        public void test_Does_SendGet_as_GET()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new SendGet {Id = 1};
            var response = client.Send(request);

            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Get));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendGet"));
        }

        [Test]
        public void test_Does_SendPost_as_POST()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new SendPost { Id = 1 };
            var response = client.Send(request);
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Post));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendPost"));
        }

        [Test]
        public void test_Does_SendPut_as_PUT()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new SendPut { Id = 1};
            var response = client.Send(request);
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Put));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendPut"));
        }
    }
}