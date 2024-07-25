using System.Net;

namespace ApiRevision
{
    public class ApiResponse
    {
        public object Result { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
    }
}
