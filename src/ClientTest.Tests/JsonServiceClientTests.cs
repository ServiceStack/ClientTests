using System;
using NUnit.Framework;
using ServiceStack;

namespace ClientTest.Tests
{
    [TestFixture]
    public class JsonServiceClientTests
    {
        [Test]
        public void Can_GET_HelloAll()
        {
            var client = Config.CreateClient();

            var request = new Hello { Name = "World" };

            var response = client.Get(request);

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void Can_use_request_filter()
        {
            var invoked = false;

            var client = new JsonServiceClient(Config.BaseUrl) {
                RequestFilter = req => invoked = true
            };

            var request = new Hello { Name = "World" };

            var response = client.Get(request);

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
            Assert.That(invoked);
        }

        class NonExistingService {}
    
        [Test]
        public void Does_process_missing_service_correctly()
        {
            var client = Config.CreateClient();

            try
            {
                client.Get(new NonExistingService());
                Assert.Fail("Should throw");
            }
            catch (WebServiceException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(405));
            }

        }

        [Test]
        public void Can_serialize_dates_correctly_via_get_request()
        {
            var client = Config.CreateClient();

            var request = new EchoTypes { DateTime = new DateTime(2015, 1, 1) };

            var response = client.Get(request);

            Assert.That(response.DateTime, Is.EqualTo(request.DateTime));
        }
    }
}