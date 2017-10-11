using System;
using System.Collections.Generic;
using Android.Util;
using Java.Math;
using Java.Util;
using NUnit.Framework;
using ServiceStack;
using Test.ServiceModel;
using Test.ServiceModel.Types;

namespace ClientTest
{
    [TestFixture]
    class TestServiceTests
    {
        private string clientUrl = "http://test.servicestack.net";

        [Test]
        public void test_Can_GET_Hello()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new Hello() {Name = "World"};

            var response = client.Get<HelloResponse>(request);

            Assert.That("Hello, World!", Is.EqualTo(response.Result));
        }

        [Test]
        public void test_does_fire_Request_and_Response_Filters()
        {
            var client = new JsonServiceClient(clientUrl);
            var events = new List<string>();

            JsonServiceClient.GlobalRequestFilter = webRequest => events.Add("GlobalRequestFilter");

            JsonServiceClient.GlobalResponseFilter = webRequest => events.Add("GlobalResponseFilter");

            client.RequestFilter = webRequest => events.Add("RequestFilter");
            client.ResponseFilter = webRequest => events.Add("ResponseFilter");

            var request = new Hello {Name = "World"};

            var response = client.Get<HelloResponse>(request);

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));

            Assert.That(events,
                Is.EquivalentTo(new string[]
                    {"RequestFilter", "GlobalRequestFilter", "ResponseFilter", "GlobalResponseFilter"}));
        }

        [Test]
        public void test_Can_GET_Hello_with_CustomPath()
        {
            var client = new JsonServiceClient(clientUrl);
            var response = client.Get<HelloResponse>("/hello/World");

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void test_Can_POST_Hello_with_CustomPath()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new Hello {Name = "World"};

            var response = client.Post<HelloResponse>("/hello");

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void test_Can_GET_Hello_with_CustomPath_raw()
        {
            var client = new JsonServiceClient(clientUrl);
            var response = client.Get("/hello/World");
            var json = response.ReadToEnd();

            Assert.That(json, Is.EqualTo("{\"result\":\"Hello, World!\"}"));
        }

        /*       [Test]
               public void test_Can_POST_Hello_with_CustomPath_raw()
               {
                   var client = new JsonServiceClient(clientUrl);
                   var response = client.Post("/hello", "Name=World".ToUtf8Bytes());
                   var json = response.ReadToEnd();

                   Assert.That(json, Is.EqualTo("{\"result\":\"Hello, World!\"}"));
               }
       */

        private HelloAllTypes CreateHelloAllTypes() => new HelloAllTypes
        {
            Name = "name",
            AllTypes = CreateAllTypes(),
            AllCollectionTypes = CreateAllCollectionTypes()
        };


        AllTypes CreateAllTypes() =>
            new AllTypes
            {
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
                DateTime = new DateTime(101, 0, 1),
                DateTimeOffset = new DateTimeOffset(new DateTime(101, 0, 1), TimeSpan.Zero),
                TimeSpan = TimeSpan.FromHours(1),
                Guid = new Guid(),
                StringList = new List<string>() {"A", "B", "C"},
                StringArray = new string[] {"D", "E", "F"},
                StringMap = new Dictionary<string, string>
                {
                    {"A", "D"},
                    {"B", "E"},
                    {"C", "F"}
                },
                IntStringMap = new Dictionary<int, string> {{1, "A"}, {2, "B"}, {3, "C"}},
                SubType = new SubType {Id = 1, Name = "name"}
            };

        AllCollectionTypes CreateAllCollectionTypes() =>
            new AllCollectionTypes
            {
                IntArray = new[] {1, 2, 3,},
                IntList = new List<int> {4, 5, 6},
                StringArray = new[] {"A", "B", "C"},
                StringList = new List<string> {"D", "E", "F"},
                PocoArray = new Poco[] {CreatePoco("pocoArray")},
                PocoList = new List<Poco> {CreatePoco("pocoList")},
                PocoLookup =
                    new Dictionary<string, List<Poco>> {{"A", new List<Poco>() {CreatePoco("B"), CreatePoco("C")}}},
                PocoLookupMap = new Dictionary<string, List<Dictionary<string, Poco>>>()
                {
                    {
                        "A",
                        new List<Dictionary<string, Poco>>
                        {
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
            DtoHelper.AssertAllTypes(actual.AllTypes, expected.AllTypes);
            DtoHelper.AssertAllCollectionTypes(actual.AllCollectionTypes, expected.AllCollectionTypes);
        }

        [Test]
        public void test_Can_POST_test_HelloAllTypes()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = CreateHelloAllTypes();
            var response = client.Post<HelloAllTypesResponse>(request);
            AssertHelloAllTypesResponse(response, request);
        }

        [Test]
        public void test_Can_PUT_test_HelloAllTypes()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = CreateHelloAllTypes();
            var response = client.Put<HelloAllTypesResponse>(request);
            AssertHelloAllTypesResponse(response, request);
        }

 /*       public void test_Can_Serailize_AllTypes()
        {
            var client = new JsonServiceClient(clientUrl);
            var json = client.gson.toJson(CreateAllTypes());
        }*/

/*        public void test_Does_handle_404_Error()
        {
            var client = new JsonServiceClient(clientUrl);

            var globalError:
            Exception ? = null
            var localError:
            Exception ? = null

            var thrownError:
            WebServiceException ? = null
            

            JsonServiceClient.GlobalExceptionFilter = ExceptionFilter {
                res, ex->globalError = ex
            }

            testClient.ExceptionFilter = ExceptionFilter {
                res, ex->localError = ex
            }

            val request = ThrowType()
            request.Type = "NotFound"
            request.message = "not here"

            try
            {
                val response = testClient.put<ThrowTypeResponse>(request)
            }
            catch (webEx:
            WebServiceException) {
                thrownError = webEx
            }

            Assert.assertNotNull(globalError)
            Assert.assertNotNull(localError)
            Assert.assertNotNull(thrownError)

            val status = thrownError!!.responseStatus

            Assert.assertEquals("NotFound", status.errorCode)
            Assert.assertEquals("not here", status.message)
            Assert.assertNotNull(status.stackTrace)
        }
        */
        public void test_Does_handle_ValidationException()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new ThrowValidation() { Email = "invalidemail" };

            try
            {
                client.Post<ThrowValidationResponse>(request);
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

        public void test_Can_POST_valid_ThrowValidation_request()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new ThrowValidation() {Age = 21, Required = "foo", Email = "my@gmail.com"};

            var response = client.Post<ThrowValidationResponse>(request);

            Assert.IsNotNull(response);
            Assert.That(response.Age, Is.EqualTo(request.Age));
            Assert.That(response.Required, Is.EqualTo(request.Required));
            Assert.That(response.Email, Is.EqualTo(request.Email));
        }

/*        public void test_does_handle_auth_failure()
        {
            var techStacksClient = new JsonServiceClient("http://techstacks.io/");
            int errorCode = 0;
            try
            {
                var request = new LockTechStack();
                request.TechnologyStackId = 6;
                var res = techStacksClient.Post(request)
                Assert.Fail("Should throw")
            }
            catch (WebServiceException ex) {
                //private StatusCode has correct code, response status is null due to empty response body.
                errorCode = ex.StatusCode;
            }

            Assert.That(errorCode, Is.EqualTo(401));
        }
        public void test_Can_send_ReturnVoid()
        {
            var client = new JsonServiceClient(clientUrl);
            var sentMethods = new List<String>();
            client.RequestFilter = ConnectionFilter {
                conn->sentMethods.add(conn.requestMethod)
            }

            val request = HelloReturnVoid()
            request.id = 1

            client.send(request)
            Assert.assertEquals(HttpMethods.Post, sentMethods[sentMethods.size - 1])
            request.id = 2
            client.get(request)
            Assert.assertEquals(HttpMethods.Get, sentMethods[sentMethods.size - 1])
            request.id = 3
            client.post(request)
            Assert.assertEquals(HttpMethods.Post, sentMethods[sentMethods.size - 1])
            request.id = 4
            client.put(request)
            Assert.assertEquals(HttpMethods.Put, sentMethods[sentMethods.size - 1])
            request.id = 5
            client.delete(request)
            Assert.assertEquals(HttpMethods.Delete, sentMethods[sentMethods.size - 1])
        }
        */

        [Test]
        public void test_Can_get_response_as_Raw_String()
        {
            var client = new JsonServiceClient(clientUrl);
            var request = new HelloString {Name = "World"};
            var response = client.Get(request);
            Assert.That(response, Is.EqualTo("World"));
        }

        [Test]
        public void test_Can_get_response_as_Raw_Bytes()
        {
            var client = new JsonServiceClient(clientUrl);
            var response = client.Get<byte[]>("/json/reply/HelloString?Name=World");
            Assert.That(response.FromUtf8Bytes(), Is.EqualTo("World"));
        }

    } //Log.Instance = new AndroidLogProvider("ZZZ");
}