using System;

namespace Jal.HttpClient
{
    public interface IHttpHeaderDescriptor
    {
        void Add(string name, string value);
    }
}