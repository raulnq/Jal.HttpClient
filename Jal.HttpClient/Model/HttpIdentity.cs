namespace Jal.HttpClient.Model
{
    public class HttpIdentity
    {
        public string OperationId { get; set; }
        public string ParentId { get; set; }
        public string Id { get; set; }
        public HttpIdentity(string id)
        {
            Id = id;
        }
    }
}