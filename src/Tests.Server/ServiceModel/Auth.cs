using ServiceStack;

namespace Tests.ServiceModel
{
    [Route("/testauth")]
    public class TestAuth : IReturn<TestAuthResponse> { }

    public class TestAuthResponse
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class RequiresAdmin : IReturn<RequiresAdmin>
    {
        public int Id { get; set; }
    }
}