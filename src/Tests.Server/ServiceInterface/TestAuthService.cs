using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.Auth;
using Tests.ServiceModel;

namespace Tests.ServiceInterface
{
    public class CustomUserSession : AuthUserSession
    {
        [DataMember]
        public string CustomName { get; set; }

        [DataMember]
        public string CustomInfo { get; set; }
    }

    [Authenticate]
    public class TestAuthService : Service
    {
        public object Any(TestAuth request)
        {
            var session = base.SessionAs<CustomUserSession>();
            return new TestAuthResponse {
                UserId = session.UserAuthId,
                UserName = session.UserAuthName,
                DisplayName = session.DisplayName
                    ?? session.UserName
                    ?? $"{session.FirstName} {session.LastName}".Trim(),
                SessionId = session.Id,
            };
        }
    }

    [RequiredRole("Admin")]
    public class AdminServices : Service
    {
        public object Any(RequiresAdmin request) => request;
    }
}