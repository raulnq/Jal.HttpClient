using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Jal.HttpClient.Model
{
    public class HttpRequestMultiPartFormDataContent : HttpRequestContent
    {
        public List<HttpRequestSimpleDataContent> Contents { get; set; }

        private string _boundary; 

        public HttpRequestMultiPartFormDataContent()
        {
            Contents = new List<HttpRequestSimpleDataContent>();
        }

        public override string ToString()
        {
            return "[MultiPartFormData]";
        }

        public override void WriteStream(Stream writeStream)
        {
            var needsClrf = false;

            foreach (var content in Contents)
            {
                if (needsClrf)
                {
                    var clrfbytes = GetDefaultEncoding().GetBytes("\r\n");

                    writeStream.Write(clrfbytes, 0, clrfbytes.Length);
                }


                needsClrf = true;

                var contenttype = content.GetContentType();

                var encoding = content.GetEncoding();

                if (!string.IsNullOrWhiteSpace(content.Disposition.FileName))
                {
                    var header = $"--{_boundary}\r\nContent-Disposition: form-data; name=\"{content.Disposition.Name}\"; filename=\"{content.Disposition.FileName}\"\r\nContent-Type: {content.ContentType ?? GetDefaultContentType()}\r\n\r\n";

                    var bytes = encoding.GetBytes(header);

                    writeStream.Write(bytes, 0, bytes.Length);

                    content.WriteStream(writeStream);
                }
                else
                {
                    string header = $"--{_boundary}\r\nContent-Disposition: form-data; name=\"{content.Disposition.Name}\"\r\n\r\n";

                    var bytes = encoding.GetBytes(header);

                    var length = encoding.GetByteCount(header);

                    writeStream.Write(bytes, 0, length);

                    content.WriteStream(writeStream);
                }
            }

            var footer = "\r\n--" + _boundary + "--\r\n";

            var footerbytes = GetDefaultEncoding().GetBytes(footer);

            writeStream.Write(footerbytes, 0, footerbytes.Length);

            writeStream.Flush();
        }

        public override void Write(WebRequest request)
        {
            if (Contents.Any())
            {
                _boundary = $"----------{Guid.NewGuid():N}";

                request.ContentType = "multipart/form-data; boundary=" + _boundary;

                request.ContentLength = GetByteCount();

                using (var writeStream = request.GetRequestStream())
                {
                    WriteStream(writeStream);
                }
            }
        }

        public override long GetByteCount()
        {
            long total = 0;

            var needsClrf = false;

            foreach (var content in Contents)
            {
                if (needsClrf)
                {
                    var clrfbytes = GetDefaultEncoding().GetByteCount("\r\n");

                    total = total + clrfbytes;
                }

                needsClrf = true;

                var contenttype = content.GetContentType();

                var encoding = content.GetEncoding();

                if (!string.IsNullOrWhiteSpace(content.Disposition.FileName))
                {
                    var header = $"--{_boundary}\r\nContent-Disposition: form-data; name=\"{content.Disposition.Name}\"; filename=\"{content.Disposition.FileName}\"\r\nContent-Type: {content.ContentType ?? GetDefaultContentType()}\r\n\r\n";

                    var bytes = encoding.GetByteCount(header);

                    total = total + bytes;

                    total = total + content.GetByteCount();
                }
                else
                {
                    string header = $"--{_boundary}\r\nContent-Disposition: form-data; name=\"{content.Disposition.Name}\"\r\n\r\n";

                    var bytes = encoding.GetByteCount(header);

                    total = total + bytes;

                    total = total + content.GetByteCount();
                }
            }

            var footer = "\r\n--" + _boundary + "--\r\n";

            var footerbytes = GetDefaultEncoding().GetByteCount(footer);

            total = total + footerbytes;

            return total;
        }

        public override string GetDefaultContentType()
        {
            return ApplicationOctectStream;
        }

        public override Encoding GetDefaultEncoding()
        {
            return Encoding.UTF8;
        }
    }
}