using System;
using NUnit.Framework;
using ServiceStack;
using Test.ServiceInterface;
using Test.ServiceModel;

namespace ClientTest
{
    [TestFixture]
    public class JsonServiceClientTests
    {
        private string clientUrl = Config.BaseUrl;

        public void test_can_GET_HelloAll()
        {
            var client = new JsonServiceClient(clientUrl);

            var request = new Hello() { Name = "World" };

            var response = client.Get(request);

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

 /*       public void test_can_use_request_filter()
        {
            var client = new JsonServiceClient(clientUrl);

            //var passTest = arrayOf(false)

            client.RequestFilter = new ConnectionFilter
            {
                passTest[0] = true
            };

            var request = new Hello() { Name = "World" };

            var response = client.Get(request);

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
            Assert.That(passTest[0], Is.True);
        }
        */

        [Test]
        public void test_does_process_missing_service_correctly()
        {
            var client = new JsonServiceClient(clientUrl);

            try
            {
                client.Get(new EchoTypes());
                Assert.Fail("Should throw");
            }
            catch (WebServiceException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(405));
            }

        }

        [Test]
        public void test_can_serialize_dates_correctly_via_get_request()
        {
            var client = new JsonServiceClient(clientUrl);

            var request = new EchoTypes { DateTime = new DateTime(2015, 1, 1) };

            var response = client.Get(request);

            Assert.That(response.DateTime, Is.EqualTo(request.DateTime));
        }
    }
}