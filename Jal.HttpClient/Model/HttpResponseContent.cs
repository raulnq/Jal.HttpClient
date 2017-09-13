using System;
using System.IO;
using System.Text;

namespace Jal.HttpClient.Model
{
    public class HttpResponseContent : IDisposable
    {
        public MemoryStream Stream { get; set; }

        public long ContentLength { get; set; }

        public string ContentType { get; set; }

        public string CharacterSet { get; set; }

        public bool IsString()
        {
            return !string.IsNullOrWhiteSpace(ContentType) && (ContentType.Contains("text") || ContentType.Contains("xml") || ContentType.Contains("json") || ContentType.Contains("html"));
        }

        public string Read()
        {
            var content = string.Empty;

            if (Stream != null && Stream.CanRead)
            {
                var encoding = string.IsNullOrEmpty(CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(CharacterSet);

                content = encoding.GetString(Stream.ToArray());
            }

            return content;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stream?.Dispose();
                Stream = null;
            }
        }
    }
}