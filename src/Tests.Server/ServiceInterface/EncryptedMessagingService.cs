using System;
using System.Collections.Generic;
using ServiceStack;

namespace Tests.ServiceInterface
{
    public class HelloSecure : IReturn<HelloSecureResponse>
    {
        public string Name { get; set; }
    }

    public class HelloSecureResponse
    {
        public string Result { get; set; }
    }

    public class GetSecure : IReturn<GetSecureResponse>
    {
        public string Name { get; set; }
    }

    public class GetSecureResponse
    {
        public string Result { get; set; }
    }

    public class HelloAuthenticated : IReturn<HelloAuthenticatedResponse>, IHasSessionId, IHasVersion
    {
        public string SessionId { get; set; }
        public int Version { get; set; }
    }

    public class LargeMessage : IReturn<LargeMessage>
    {
        public List<HelloSecure> Messages { get; set; }
    }

    [Authenticate]
    public class HelloAuthSecure : IReturn<HelloAuthSecureResponse>
    {
        public string Name { get; set; }
    }

    public class HelloAuthSecureResponse
    {
        public string Result { get; set; }
    }

    public class HelloAuthenticatedResponse
    {
        public int Version { get; set; }
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class HelloOneWay : IReturnVoid
    {
        internal static string LastName;

        public string Name { get; set; }
    }

    public class SecureServices : Service
    {
        public object Get(GetSecure request)
        {
            if (request.Name == null)
                throw new ArgumentNullException("Name");

            return new GetSecureResponse { Result = "Hello, {0}!".Fmt(request.Name) };
        }

        public object Any(HelloSecure request)
        {
            if (request.Name == null)
                throw new ArgumentNullException("Name");

            return new HelloSecureResponse { Result = "Hello, {0}!".Fmt(request.Name) };
        }

        public object Any(HelloAuthSecure request)
        {
            if (request.Name == null)
                throw new ArgumentNullException("Name");

            return new HelloAuthSecureResponse { Result = "Hello, {0}!".Fmt(request.Name) };
        }

        [Authenticate]
        public object Any(HelloAuthenticated request)
        {
            var session = GetSession();

            return new HelloAuthenticatedResponse
            {
                Version = request.Version,
                SessionId = session.Id,
                UserName = session.UserName,
                Email = session.Email,
                IsAuthenticated = session.IsAuthenticated,
            };
        }

        public object Any(LargeMessage request)
        {
            return request;
        }

        public void Any(HelloOneWay request)
        {
            HelloOneWay.LastName = request.Name;
        }
    }
    
}