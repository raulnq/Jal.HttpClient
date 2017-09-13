namespace Jal.HttpClient.Model
{
    public class HttpHeader
    {
        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}