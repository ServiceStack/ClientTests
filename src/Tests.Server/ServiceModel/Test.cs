using ServiceStack;

namespace Tests.ServiceModel
{
    [Route("/void-response")]
    public class TestVoidResponse { }

    [Route("/null-response")]
    public class TestNullResponse { }

    [Route("/wait/{ForMs}")]
    public class Wait : IReturn<Wait>
    {
        public int ForMs { get; set; }
    }

}