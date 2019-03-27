namespace Jal.HttpClient.Model
{
    public class HttpQueryParameter
    {
        public HttpQueryParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; }

        public string Value { get;  }
    }
}