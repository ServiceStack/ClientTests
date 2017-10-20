using ServiceStack;
using Tests.ServiceModel;

namespace Tests.ServiceInterface
{
    public class EchoService : Service
    {
        public object Any(EchoTypes request) => request;

        public object Any(EchoCollections request) => request;

        public object Any(EchoComplexTypes request) => request;
    }
}