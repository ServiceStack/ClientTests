using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using ServiceStack;

namespace ClientTest.Tests
{
    [TestFixture]
    class TestServiceTests
    {
        [Test]
        public void Can_GET_Hello()
        {
            var client = Config.CreateClient();
            var request = new Hello {Name = "World"};

            var response = client.Get(request);

            Assert.That("Hello, World!", Is.EqualTo(response.Result));
        }

        [Test]
        public void Does_fire_Request_and_Response_Filters()
        {
            var client = Config.CreateClient();
            var events = new List<string>();

            JsonServiceClient.GlobalRequestFilter = webRequest => events.Add("GlobalRequestFilter");

            JsonServiceClient.GlobalResponseFilter = webRequest => events.Add("GlobalResponseFilter");

            client.RequestFilter = webRequest => events.Add("RequestFilter");
            client.ResponseFilter = webRequest => events.Add("ResponseFilter");

            var request = new Hello {Name = "World"};

            var response = client.Get(request);

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));

            Assert.That(events, Is.EquivalentTo(new[]
                {"RequestFilter", "GlobalRequestFilter", "ResponseFilter", "GlobalResponseFilter"}));

            JsonServiceClient.GlobalRequestFilter = null;
            JsonServiceClient.GlobalResponseFilter = null;
        }

        [Test]
        public void Can_GET_Hello_with_CustomPath()
        {
            var client = Config.CreateClient();
            var response = client.Get<HelloResponse>("/hello/World");

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void Can_POST_Hello_with_CustomPath()
        {
            var client = Config.CreateClient();
            var request = new Hello {Name = "World"};

            var response = client.Post<HelloResponse>("/hello", request);

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void Can_GET_Hello_with_CustomPath_raw()
        {
            var client = Config.CreateClient();
            var response = client.Get("/hello/World");
            var json = response.ReadToEnd();

            Assert.That(json, Is.EqualTo("{\"result\":\"Hello, World!\"}"));
        }

        [Test]
        public void Can_POST_Hello_with_CustomPath_raw()
        {
            var client = Config.CreateClient();
            var response = client.Post<Stream>("/hello", "{\"Name\":\"World\"}".ToUtf8Bytes());
            var json = response.ReadFully().FromUtf8Bytes();

            Assert.That(json, Is.EqualTo("{\"result\":\"Hello, World!\"}"));
        }

        
        private HelloAllTypes CreateHelloAllTypes() => new HelloAllTypes {
            Name = "name",
            AllTypes = CreateAllTypes(),
            AllCollectionTypes = CreateAllCollectionTypes()
        };

        AllTypes CreateAllTypes() =>
            new AllTypes {
                Id = 1,
                Char = 'c',
                Byte = 2,
                Short = 3,
                Int = 4,
                Long = 5,
                UShort = 6,
                UInt = 7,
                ULong = 8,
                Float = 1.1f,
                Double = 2.2,
                Decimal = 3.0m,
                String = "string",
                DateTime = new DateTime(101, 1, 1),
                DateTimeOffset = new DateTimeOffset(new DateTime(101, 1, 1), TimeSpan.Zero),
                TimeSpan = TimeSpan.FromHours(1),
                Guid = new Guid(),
                StringList = new List<string>() {"A", "B", "C"},
                StringArray = new string[] {"D", "E", "F"},
                StringMap = new Dictionary<string, string> {
                    {"A", "D"},
                    {"B", "E"},
                    {"C", "F"}
                },
                IntStringMap = new Dictionary<int, string> {{1, "A"}, {2, "B"}, {3, "C"}},
                SubType = new SubType {Id = 1, Name = "name"}
            };

        AllCollectionTypes CreateAllCollectionTypes() =>
            new AllCollectionTypes {
                IntArray = new[] {1, 2, 3,},
                IntList = new List<int> {4, 5, 6},
                StringArray = new[] {"A", "B", "C"},
                StringList = new List<string> {"D", "E", "F"},
                PocoArray = new Poco[] {CreatePoco("pocoArray")},
                PocoList = new List<Poco> {CreatePoco("pocoList")},
                PocoLookup =
                    new Dictionary<string, List<Poco>> {{"A", new List<Poco>() {CreatePoco("B"), CreatePoco("C")}}},
                PocoLookupMap = new Dictionary<string, List<Dictionary<string, Poco>>>() {
                    {
                        "A",
                        new List<Dictionary<string, Poco>> {
                            new Dictionary<string, Poco> {{"B", CreatePoco("C")}},
                            new Dictionary<string, Poco> {{"D", CreatePoco("E")}},
                        }
                    }
                }
            };

        Poco CreatePoco(string name) => new Poco {Name = name};

        void AssertHelloAllTypesResponse(HelloAllTypesResponse actual, HelloAllTypes expected)
        {
            Assert.IsNotNull(actual);
            TestUtils.AssertAllTypes(actual.AllTypes, expected.AllTypes);
            TestUtils.AssertAllCollectionTypes(actual.AllCollectionTypes, expected.AllCollectionTypes);
        }

        [Test]
        public void Can_POST_HelloAllTypes()
        {
            var client = Config.CreateClient();
            var request = CreateHelloAllTypes();
            var response = client.Post(request);
            AssertHelloAllTypesResponse(response, request);
        }

        [Test]
        public void Can_PUT_HelloAllTypes()
        {
            var client = Config.CreateClient();
            var request = CreateHelloAllTypes();
            var response = client.Put(request);
            AssertHelloAllTypesResponse(response, request);
        }

        [Test]
        public void Can_Serailize_AllTypes()
        {
            var client = Config.CreateClient();
            var json = CreateAllTypes().ToJson();
        }

        [Test]
        public void Does_handle_404_Error()
        {
            var client = Config.CreateClient();

            var request = new ThrowType {
                Type = "NotFound",
                Message = "not here",
            };

            try
            {
                var response = client.Put(request);
            }
            catch (WebServiceException ex)
            {
                var status = ex.GetResponseStatus();
                Assert.That(status.ErrorCode, Is.EqualTo("NotFound"));
                Assert.That(status.Message, Is.EqualTo("not here"));
                Assert.That(status.StackTrace, Is.Not.Null);
            }
        }

        [Test]
        public void Does_handle_ValidationException()
        {
            var client = Config.CreateClient();
            var request = new ThrowValidation {Email = "invalidemail"};

            try
            {
                client.Post(request);
                Assert.Fail("Should throw");
            }
            catch (WebServiceException webEx)
            {
                var status = webEx.ResponseStatus;

                Assert.IsNotNull(status);
                Assert.That(status.Errors.Count, Is.EqualTo(3));

                Assert.That(status.Errors[0].ErrorCode, Is.EqualTo(status.ErrorCode));
                Assert.That(status.Errors[0].Message, Is.EqualTo(status.Message));

                Assert.That(status.Errors[0].ErrorCode, Is.EqualTo("InclusiveBetween"));
                Assert.That(status.Errors[0].Message, Is.EqualTo("'Age' must be between 1 and 120. You entered 0."));
                Assert.That(status.Errors[0].FieldName, Is.EqualTo("Age"));

                Assert.That(status.Errors[1].ErrorCode, Is.EqualTo("NotEmpty"));
                Assert.That(status.Errors[1].Message, Is.EqualTo("'Required' should not be empty."));
                Assert.That(status.Errors[1].FieldName, Is.EqualTo("Required"));

                Assert.That(status.Errors[2].ErrorCode, Is.EqualTo("Email"));
                Assert.That(status.Errors[2].Message, Is.EqualTo("'Email' is not a valid email address."));
                Assert.That(status.Errors[2].FieldName, Is.EqualTo("Email"));
            }
        }

        [Test]
        public void Can_POST_valid_ThrowValidation_request()
        {
            var client = Config.CreateClient();
            var request = new ThrowValidation {Age = 21, Required = "foo", Email = "my@gmail.com"};

            var response = client.Post(request);

            Assert.IsNotNull(response);
            Assert.That(response.Age, Is.EqualTo(request.Age));
            Assert.That(response.Required, Is.EqualTo(request.Required));
            Assert.That(response.Email, Is.EqualTo(request.Email));
        }

        [Test]
        public void Does_handle_auth_failure()
        {
            var client = Config.CreateClient();
            try
            {
                var request = new RequiresAdmin();
                var res = client.Post(request);
                Assert.Fail("Should throw");
            }
            catch (WebServiceException ex)
            {
                //private StatusCode has correct code, response status is null due to empty response body.
                Assert.That(ex.StatusCode, Is.EqualTo(401));
            }
        }

        [Test]
        public void Can_send_ReturnVoid()
        {
            var sentMethods = new List<string>();
            var client = new JsonServiceClient(Config.BaseUrl) {
                RequestFilter = req => sentMethods.Add(req.Method)
            };

            var request = new SendReturnVoid {Id = 1};

            client.Send(request);
            Assert.That(sentMethods[sentMethods.Count - 1], Is.EqualTo(HttpMethods.Post));
            request.Id = 2;
            client.Get(request);
            Assert.That(sentMethods[sentMethods.Count - 1], Is.EqualTo(HttpMethods.Get));
            request.Id = 3;
            client.Post(request);
            Assert.That(sentMethods[sentMethods.Count - 1], Is.EqualTo(HttpMethods.Post));
            request.Id = 4;
            client.Put(request);
            Assert.That(sentMethods[sentMethods.Count - 1], Is.EqualTo(HttpMethods.Put));
            request.Id = 5;
            client.Delete(request);
            Assert.That(sentMethods[sentMethods.Count - 1], Is.EqualTo(HttpMethods.Delete));
        }

        [Test]
        public void Can_get_response_as_Raw_String()
        {
            var client = Config.CreateClient();
            var request = new HelloString {Name = "World"};
            var response = client.Get(request);
            Assert.That(response, Is.EqualTo("World"));
        }

        [Test]
        public void Can_get_response_as_Raw_Bytes()
        {
            var client = Config.CreateClient();
            var response = client.Get<byte[]>("/json/reply/HelloString?Name=World");
            Assert.That(response.FromUtf8Bytes(), Is.EqualTo("World"));
        }
    }
}