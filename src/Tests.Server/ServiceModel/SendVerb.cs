using ServiceStack;

namespace Tests.ServiceModel
{
    public class SendVerbResponse
    {
        public int Id { get; set; }
        public string PathInfo { get; set; }
        public string RequestMethod { get; set; }
    }

    public class SendDefault : IReturn<SendVerbResponse>
    {
        public int Id { get; set; }
    }

    [Route("/sendrestget/{Id}", "GET")]
    public class SendRestGet : IReturn<SendVerbResponse>, IGet
    {
        public int Id { get; set; }
    }

    public class SendGet : IReturn<SendVerbResponse>, IGet
    {
        public int Id { get; set; }
    }

    public class SendPost : IReturn<SendVerbResponse>, IPost
    {
        public int Id { get; set; }
    }

    public class SendPut : IReturn<SendVerbResponse>, IPut
    {
        public int Id { get; set; }
    }

    public class SendReturnVoid : IReturnVoid
    {
        public int Id { get; set; }
    }
}