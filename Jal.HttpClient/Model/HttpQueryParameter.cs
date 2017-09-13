namespace Jal.HttpClient.Model
{
    public class HttpQueryParameter
    {
        public HttpQueryParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }

        public string Value { get; set; }
    }
}