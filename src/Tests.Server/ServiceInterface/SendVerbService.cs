using ServiceStack;
using Tests.ServiceModel;

namespace Tests.ServiceInterface
{
    public class SendVerbService : Service
    {
        public object Any(SendDefault request)
        {
            return CreateResponse(request.Id);
        }

        public object Get(SendRestGet request)
        {
            return CreateResponse(request.Id);
        }

        public object Any(SendGet request)
        {
            return CreateResponse(request.Id);
        }

        public object Any(SendPost request)
        {
            return CreateResponse(request.Id);
        }

        public object Any(SendPut request)
        {
            return CreateResponse(request.Id);
        }

        private object CreateResponse(int requestId)
        {
            return new SendVerbResponse {
                Id = requestId,
                PathInfo = base.Request.PathInfo,
                RequestMethod = base.Request.Verb
            };
        }
        
        public void Any(SendReturnVoid request) {}
    }
}