using System.Threading.Tasks;
using NUnit.Framework;
using ServiceStack;

namespace ClientTest.Tests
{
    [TestFixture]
    public class TestInterfaceMarkerTestsAsync
    {
        [Test]
        public async Task Does_SendDefault_as_POST()
        {
            var client = Config.CreateClient();

            var request = new SendDefault {Id = 1};

            var response = await client.SendAsync<SendVerbResponse>(request).ConfigureAwait(false);

            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Post));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendDefault"));
        }

        [Test]
        public async Task Does_SendRestGet_as_GET_using_Predefined_Route()
        {
            var client = Config.CreateClient();
            var request = new SendRestGet {Id = 1};

            var response = await client.SendAsync(request).ConfigureAwait(false);

            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Get));
            Assert.That(response.PathInfo, Is.EqualTo("/sendrestget/1"));
        }

        [Test]
        public async Task Does_SendGet_as_GET()
        {
            var client = Config.CreateClient();
            var request = new SendGet {Id = 1};
            var response = await client.SendAsync(request).ConfigureAwait(false);

            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Get));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendGet"));
        }

        [Test]
        public async Task Does_SendPost_as_POST()
        {
            var client = Config.CreateClient();
            var request = new SendPost {Id = 1};
            var response = await client.SendAsync(request).ConfigureAwait(false);

            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Post));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendPost"));
        }

        [Test]
        public async Task Does_SendPut_as_PUT()
        {
            var client = Config.CreateClient();
            var request = new SendPut {Id = 1};
            var response = await client.SendAsync(request).ConfigureAwait(false);
            Assert.That(response.Id, Is.EqualTo(1));
            Assert.That(response.RequestMethod, Is.EqualTo(HttpMethods.Put));
            Assert.That(response.PathInfo, Is.EqualTo("/json/reply/SendPut"));
        }
    }
}