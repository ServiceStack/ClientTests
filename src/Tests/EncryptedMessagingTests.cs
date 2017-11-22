using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Text;

namespace Tests
{
    public class JsonServiceClientEncryptedMessagesTests : EncryptedMessagesTests
    {
        protected override IJsonServiceClient CreateClient()
        {
            return new JsonServiceClient(Config.BaseUrl);
        }
    }

    public class JsonHttpClientEncryptedMessagesTests : EncryptedMessagesTests
    {
        protected override IJsonServiceClient CreateClient()
        {
            return new JsonHttpClient(Config.BaseUrl);
        }
    }

    public abstract class EncryptedMessagesTests
    {
        protected abstract IJsonServiceClient CreateClient();

        [Test]
        public void Can_Send_Encrypted_Message_with_ServiceClients()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get(new GetPublicKey()));

            var response = encryptedClient.Send(new HelloSecure { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void Can_Send_Encrypted_OneWay_Message_with_ServiceClients()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get(new GetPublicKey()));

            encryptedClient.Send(new HelloOneWay { Name = "World" });
        }

        [Test]
        public void Can_authenticate_and_call_authenticated_Service()
        {
            try
            {
                var client = CreateClient();
                IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

                var authResponse = encryptedClient.Send(new Authenticate
                {
                    provider = "credentials",
                    UserName = "test@gmail.com",
                    Password = "test",
                });

                var encryptedClientCookies = client.GetCookieValues();
                Assert.That(encryptedClientCookies.Count, Is.EqualTo(0));

                var response = encryptedClient.Send(new HelloAuthenticated
                {
                    SessionId = authResponse.SessionId,
                });

                Assert.That(response.IsAuthenticated);
                Assert.That(response.Email, Is.EqualTo("test@gmail.com"));
                Assert.That(response.SessionId, Is.EqualTo(authResponse.SessionId));

                encryptedClientCookies = client.GetCookieValues();
                Assert.That(encryptedClientCookies.Count, Is.EqualTo(0));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Test]
        public void Does_populate_Request_metadata()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            var authResponse = encryptedClient.Send(new Authenticate
            {
                provider = "credentials",
                UserName = "test@gmail.com",
                Password = "test",
            });

            var encryptedClientCookies = client.GetCookieValues();
            Assert.That(encryptedClientCookies.Count, Is.EqualTo(0));

            encryptedClient.Version = 1;
            encryptedClient.SessionId = authResponse.SessionId;

            var response = encryptedClient.Send(new HelloAuthenticated());
            Assert.That(response.SessionId, Is.EqualTo(encryptedClient.SessionId));
            Assert.That(response.Version, Is.EqualTo(encryptedClient.Version));

            encryptedClientCookies = client.GetCookieValues();
            Assert.That(encryptedClientCookies.Count, Is.EqualTo(0));

            client.SessionId = authResponse.SessionId;
            client.Version = 2;

            response = client.Send(new HelloAuthenticated());
            Assert.That(response.SessionId, Is.EqualTo(client.SessionId));
            Assert.That(response.Version, Is.EqualTo(client.Version));
        }

        [Test]
        public void Can_Authenticate_then_call_AuthOnly_Services_with_ServiceClients_Temp()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            var authResponse = encryptedClient.Send(new Authenticate
            {
                provider = "credentials",
                UserName = "test@gmail.com",
                Password = "test",
            });

            client.SetCookie("ss-id", authResponse.SessionId);
            var response = client.Get(new HelloAuthSecure { Name = "World" });
        }

        [Test]
        public void Can_Authenticate_then_call_AuthOnly_Services_with_ServiceClients_Perm()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            var authResponse = encryptedClient.Send(new Authenticate
            {
                provider = "credentials",
                UserName = "test@gmail.com",
                Password = "test",
                RememberMe = true,
            });

            client.SetCookie("ss-pid", authResponse.SessionId);
            client.SetCookie("ss-opt", "perm");
            var response = client.Get(new HelloAuthSecure { Name = "World" });
        }

        [Test]
        public void Does_handle_Exceptions()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            try
            {
                var response = encryptedClient.Send(new HelloSecure());
                Assert.Fail("Should throw");
            }
            catch (WebServiceException ex)
            {
                Assert.That(ex.ResponseStatus.ErrorCode, Is.EqualTo(typeof(ArgumentNullException).Name));
                Assert.That(ex.ResponseStatus.Message.NormalizeNewLines(), Is.EqualTo($"Value cannot be null.\nParameter name: Name"));
            }

            try
            {
                var response = encryptedClient.Send(new HelloAuthenticated());
                Assert.Fail("Should throw");
            }
            catch (WebServiceException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo((int)HttpStatusCode.Unauthorized));
                Assert.That(ex.StatusDescription, Is.EqualTo("Unauthorized"));
            }
        }

        [Test]
        public void Can_call_GET_only_Services()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            var response = encryptedClient.Get(new GetSecure { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void Can_send_large_messages()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            var messages = new List<HelloSecure>();
            for (var i = 0; i < 100; i++)
            {
                messages.Add(new HelloSecure { Name = "Name" + i });
            }

            var request = new LargeMessage
            {
                Messages = messages
            };

            var response = encryptedClient.Send(request);

            Assert.That(response.Messages.Count, Is.EqualTo(request.Messages.Count));
        }

        [Test]
        public void Can_send_auto_batched_requests()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            var names = new[] { "Foo", "Bar", "Baz" };
            var requests = names.Select(x => new HelloSecure { Name = x }).ToList();

            var responses = encryptedClient.SendAll(requests);
            var responseNames = responses.Select(x => x.Result);

            Assert.That(responseNames, Is.EqualTo(names.Select(x => "Hello, {0}!".Fmt(x))));
        }

        [Test]
        public void Can_send_PublishAll_requests()
        {
            var client = CreateClient();
            IEncryptedClient encryptedClient = client.GetEncryptedClient(client.Get<string>("/publickey"));

            var names = new[] { "Foo", "Bar", "Baz" };
            IEnumerable<HelloSecure> requests = names.Select(x => new HelloSecure { Name = x }).ToList();

            encryptedClient.PublishAll(requests);
        }

        [Test]
        public void Can_Send_Encrypted_Message()
        {
            var client = CreateClient();

            var request = new HelloSecure { Name = "World" };

            AesUtils.CreateCryptAuthKeysAndIv(out var cryptKey, out var authKey, out var iv);

            var cryptAuthKeys = cryptKey.Combine(authKey);

            var rsaEncCryptAuthKeys = RsaUtils.Encrypt(cryptAuthKeys, SecureConfig.PublicKeyXml);
            var authRsaEncCryptAuthKeys = HmacUtils.Authenticate(rsaEncCryptAuthKeys, authKey, iv);

            var timestamp = DateTime.UtcNow.ToUnixTime();
            var requestBody = timestamp + " POST " + typeof(HelloSecure).Name + " " + request.ToJson();

            var encryptedBytes = AesUtils.Encrypt(requestBody.ToUtf8Bytes(), cryptKey, iv);
            var authEncryptedBytes = HmacUtils.Authenticate(encryptedBytes, authKey, iv);

            var encryptedMessage = new EncryptedMessage
            {
                EncryptedSymmetricKey = Convert.ToBase64String(authRsaEncCryptAuthKeys),
                EncryptedBody = Convert.ToBase64String(authEncryptedBytes),
            };

            var encResponse = client.Post(encryptedMessage);

            authEncryptedBytes = Convert.FromBase64String(encResponse.EncryptedBody);

            if (!HmacUtils.Verify(authEncryptedBytes, authKey))
                throw new Exception("Invalid EncryptedBody");

            var decryptedBytes = HmacUtils.DecryptAuthenticated(authEncryptedBytes, cryptKey);

            var responseJson = decryptedBytes.FromUtf8Bytes();
            var response = responseJson.FromJson<HelloSecureResponse>();

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void Does_throw_on_old_messages()
        {
            var client = CreateClient();

            var request = new HelloSecure { Name = "World" };

            AesUtils.CreateCryptAuthKeysAndIv(out var cryptKey, out var authKey, out var iv);

            var cryptAuthKeys = cryptKey.Combine(authKey);

            var rsaEncCryptAuthKeys = RsaUtils.Encrypt(cryptAuthKeys, SecureConfig.PublicKeyXml);
            var authRsaEncCryptAuthKeys = HmacUtils.Authenticate(rsaEncCryptAuthKeys, authKey, iv);

            var timestamp = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(21)).ToUnixTime();

            var requestBody = timestamp + " POST " + typeof(HelloSecure).Name + " " + request.ToJson();

            var encryptedBytes = AesUtils.Encrypt(requestBody.ToUtf8Bytes(), cryptKey, iv);
            var authEncryptedBytes = HmacUtils.Authenticate(encryptedBytes, authKey, iv);

            try
            {
                var encryptedMessage = new EncryptedMessage
                {
                    EncryptedSymmetricKey = Convert.ToBase64String(authRsaEncCryptAuthKeys),
                    EncryptedBody = Convert.ToBase64String(authEncryptedBytes),
                };
                var encResponse = client.Post(encryptedMessage);

                Assert.Fail("Should throw");
            }
            catch (WebServiceException ex)
            {
                ex.StatusDescription.Print();

                var errorResponse = (EncryptedMessageResponse)ex.ResponseDto;

                authEncryptedBytes = Convert.FromBase64String(errorResponse.EncryptedBody);
                if (!HmacUtils.Verify(authEncryptedBytes, authKey))
                    throw new Exception("EncryptedBody is Invalid");

                var responseBytes = HmacUtils.DecryptAuthenticated(authEncryptedBytes, cryptKey);
                var responseJson = responseBytes.FromUtf8Bytes();
                var response = responseJson.FromJson<ErrorResponse>();
                Assert.That(response.ResponseStatus.Message, Is.EqualTo("Request too old"));
            }
        }

        [Test]
        public void Does_throw_on_replayed_messages()
        {
            var client = CreateClient();

            var request = new HelloSecure { Name = "World" };

            AesUtils.CreateKeyAndIv(out var cryptKey, out var iv);

            byte[] authKey = AesUtils.CreateKey();

            var cryptAuthKeys = cryptKey.Combine(authKey);

            var rsaEncCryptAuthKeys = RsaUtils.Encrypt(cryptAuthKeys, SecureConfig.PublicKeyXml);
            var authRsaEncCryptAuthKeys = HmacUtils.Authenticate(rsaEncCryptAuthKeys, authKey, iv);

            var timestamp = DateTime.UtcNow.ToUnixTime();
            var requestBody = timestamp + " POST " + typeof(HelloSecure).Name + " " + request.ToJson();

            var encryptedBytes = AesUtils.Encrypt(requestBody.ToUtf8Bytes(), cryptKey, iv);
            var authEncryptedBytes = HmacUtils.Authenticate(encryptedBytes, authKey, iv);

            var encryptedMessage = new EncryptedMessage
            {
                EncryptedSymmetricKey = Convert.ToBase64String(authRsaEncCryptAuthKeys),
                EncryptedBody = Convert.ToBase64String(authEncryptedBytes),
            };

            var encResponse = client.Post(encryptedMessage);

            try
            {
                client.Post(encryptedMessage);

                Assert.Fail("Should throw");
            }
            catch (WebServiceException ex)
            {
                ex.StatusDescription.Print();

                var errorResponse = (EncryptedMessageResponse)ex.ResponseDto;

                authEncryptedBytes = Convert.FromBase64String(errorResponse.EncryptedBody);
                if (!HmacUtils.Verify(authEncryptedBytes, authKey))
                    throw new Exception("EncryptedBody is Invalid");

                var responseBytes = HmacUtils.DecryptAuthenticated(authEncryptedBytes, cryptKey);
                var responseJson = responseBytes.FromUtf8Bytes();
                var response = responseJson.FromJson<ErrorResponse>();
                Assert.That(response.ResponseStatus.Message, Is.EqualTo("Nonce already seen"));
            }
        }

        [Test]
        public void Can_send_encrypted_messages_with_old_registered_PublicKey()
        {
            var client = CreateClient();
            var encryptedClient = client.GetEncryptedClient(SecureConfig.FallbackPublicKeyXml);

            var response = encryptedClient.Send(new HelloSecure { Name = "Fallback Key" });

            Assert.That(response.Result, Is.EqualTo("Hello, Fallback Key!"));
        }

        [Test]
        public void Fails_when_sending_invalid_KeyId()
        {
            var client = CreateClient();
            var encryptedClient = (EncryptedServiceClient)client.GetEncryptedClient(SecureConfig.FallbackPublicKeyXml);
            encryptedClient.KeyId = "AAAAAA";

            try
            {
                var response = encryptedClient.Send(new HelloSecure { Name = "Fallback Key" });
                Assert.Fail("Should Throw");
            }
            catch (WebServiceException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(ex.StatusDescription, Is.EqualTo("KeyNotFoundException"));
                Assert.That(ex.ResponseStatus.Message, Does.StartWith("Key with Id '"));
            }
        }
    }
}