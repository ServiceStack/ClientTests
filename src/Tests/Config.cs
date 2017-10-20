using ServiceStack;

namespace Tests
{
    public class Config
    {
        // public static string BaseUrl { get; set; } = "http://localhost:5000";
        public static string BaseUrl { get; set; } = "http://client-tests.servicestack.net";

        public static JsonServiceClient CreateClient() => new JsonServiceClient(BaseUrl);
    }
}