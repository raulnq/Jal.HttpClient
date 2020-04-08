namespace Jal.HttpClient
{
    public interface IHttpContextDescriptor
    {
        void Add(string name, object value);
    }
}