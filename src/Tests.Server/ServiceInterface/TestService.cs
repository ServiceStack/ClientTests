using System.Threading;
using ServiceStack;
using Tests.ServiceModel;

namespace Tests.ServiceInterface
{
    public class TestService : Service
    {
        public void Any(TestVoidResponse response) { }

        public object Any(TestNullResponse response) => null;

        public object Any(Wait request)
        {
            Thread.Sleep(request.ForMs);
            return request;
        }
    }
}