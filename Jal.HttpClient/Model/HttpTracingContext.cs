namespace Jal.HttpClient
{
    public class HttpTracingContext
    {
        public string OperationId { get; }
        public string ParentId { get; }
        public string RequestId { get; }
        public HttpTracingContext(string requestid, string parentid, string operationid)
        {
            RequestId = requestid;
            ParentId = parentid;
            OperationId = operationid;
        }
    }
}