using NUnit.Framework;
using ServiceStack;

namespace ClientTest.Tests
{
    [TestFixture]
    public class TestAuthTests
    {
        [Test]
        public void AuthRequired_returns_401()
        {
            try
            {
                var client = Config.CreateClient();
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
        public void Does_send_BasicAuthHeaders()
        {
            var client = Config.CreateClient();
            client.SetCredentials("test", "test");
            //client.AlwaysSendBasicAuthHeaders = true;

            var response = client.Get(new TestAuth());

            Assert.That(response.UserId, Is.EqualTo("1"));
            Assert.That(response.UserName, Is.EqualTo("test"));
            Assert.That(response.DisplayName, Is.EqualTo("test DisplayName"));
            Assert.IsNotNull(response.SessionId);
        }

        [Test]
        public void Does_transparently_send_BasicAuthHeader_on_401_response()
        {
            var client = Config.CreateClient();
            client.SetCredentials("test", "test");

            var response = client.Get(new TestAuth());

            Assert.AreEqual(response.UserId, "1");
            Assert.That(response.UserName, Is.EqualTo("test"));
            Assert.That(response.DisplayName, Is.EqualTo("test DisplayName"));
            Assert.IsNotNull(response.SessionId);
        }

        [Test]
        public void Can_authenticate_with_CredentialsAuth()
        {
            var client = Config.CreateClient();

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