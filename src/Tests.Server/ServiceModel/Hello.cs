using ServiceStack;
using Tests.ServiceModel.Types;

namespace Tests.ServiceModel
{
    [Route("/hello")]
    [Route("/hello/{Name}")]
    public class Hello : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }
    
    [Route("/all-types")]
    public class HelloAllTypes
    {
        public string Name { get; set; }
        public AllTypes AllTypes { get; set; }
        public AllCollectionTypes AllCollectionTypes { get; set; }
    }

    public class HelloAllTypesResponse
    {
        public string Result { get; set; }
        public AllTypes AllTypes { get; set; }
        public AllCollectionTypes AllCollectionTypes { get; set; }
    }

    public class HelloString : IReturn<string>
    {
        public string Name { get; set; }
    }
}