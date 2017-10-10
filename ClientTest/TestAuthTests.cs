using NUnit.Framework;
using ServiceStack;
using Test.ServiceInterface;

namespace ClientTest
{
    [TestFixture]
    public class TestAuthTests
    {
        public JsonServiceClient CreateClient()
        {
            return new JsonServiceClient("http://test.servicestack.net");
        }

        [Test]
        public void test_AuthRequired_returns_401()
        {
            try
            {
                var client = CreateClient();
                client.Get(new TestAuth());
                Assert.Fail("should throw");
            }
            catch (WebServiceException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(401));
                Assert.That(ex.StatusDescription, Is.EqualTo("Unauthorized"));
            }
        }

        [Test]
        public void test_does_send_BasicAuthHeaders()
        {
            var client = CreateClient();
            client.SetCredentials("test", "test");
            //client.AlwaysSendBasicAuthHeaders = true;

            var response = client.Get(new TestAuth());

            Assert.That(response.UserId, Is.EqualTo("1"));
            Assert.That(response.UserName, Is.EqualTo("test"));
            Assert.That(response.DisplayName, Is.EqualTo("test DisplayName"));
            Assert.IsNotNull(response.SessionId);
        }

        [Test]
        public void test_does_transparently_send_BasicAuthHeader_on_401_response()
        {
            var client = CreateClient();
            client.SetCredentials("test", "test");

            var response = client.Get(new TestAuth());

            Assert.AreEqual(response.UserId, "1");
            Assert.That(response.UserName, Is.EqualTo("test"));
            Assert.That(response.DisplayName, Is.EqualTo("test DisplayName"));
            Assert.IsNotNull(response.SessionId);
        }

        [Test]
        public void test_can_authenticate_with_CredentialsAuth()
        {
            var client = CreateClient();

            var request = new Authenticate {provider = "credentials", UserName = "test", Password = "test"};

            var authResponse = client.Post(request);

            Assert.AreEqual(authResponse.UserId, "1");
            Assert.That(authResponse.UserName, Is.EqualTo("test"));
            Assert.IsNotNull(authResponse.SessionId);

            var response = client.Get(new TestAuth());

            Assert.AreEqual(response.UserId, "1");
            Assert.That(response.UserName, Is.EqualTo("test"));
            Assert.That(response.DisplayName, Is.EqualTo("test DisplayName"));
            Assert.IsNotNull(response.SessionId);
        }
    }
}