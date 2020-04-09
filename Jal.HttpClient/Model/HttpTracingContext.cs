namespace Jal.HttpClient
{
    public class HttpTracingContext
    {
        public string OperationId { get; }
        public string ParentId { get; }
        public string RequestId { get; }
        public HttpTracingContext(string requestid, string parentid=null, string operationid=null)
        {
            RequestId = requestid;
            ParentId = parentid;
            OperationId = operationid;
        }
    }
}