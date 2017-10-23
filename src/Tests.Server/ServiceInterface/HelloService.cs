using ServiceStack;
using Tests.ServiceModel;

namespace Tests.ServiceInterface
{
    public class HelloService : Service
    {
        public object Any(Hello request) => 
            new HelloResponse { Result = $"Hello, {request.Name}!" };
        
        public object Any(HelloAllTypes request) => 
            new HelloAllTypesResponse
            {
                AllTypes = request.AllTypes,
                AllCollectionTypes = request.AllCollectionTypes,
                Result = request.Name
            };

        public object Any(HelloString request) => request.Name;
    }
}