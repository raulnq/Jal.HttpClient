using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpMultiPartFormDataContent : HttpContent
    {
        public List<HttpContent> Contents { get; set; }

        private string _boundary; 

        public HttpMultiPartFormDataContent()
        {
            Contents = new List<HttpContent>();
        }

        public override string ToString()
        {
            return "MultiPartFormData";
        }

        public override void WriteStream(Stream writeStream)
        {
            var needsClrf = false;

            foreach (var content in Contents)
            {
                if (needsClrf)
                {
                    var bytes = DefaultEncoding.GetBytes("\r\n");

                    writeStream.Write(bytes, 0, bytes.Length);
                }


                needsClrf = true;

                if (!string.IsNullOrWhiteSpace(content.Disposition.FileName))
                {
                    var header = $"--{_boundary}\r\nContent-Disposition: form-data; name=\"{content.Disposition.Name}\"; filename=\"{content.Disposition.FileName}\"\r\nContent-Type: {content.ContentType ?? "application/octet-stream"}\r\n\r\n";

                    var bytes = DefaultEncoding.GetBytes(header);

                    writeStream.Write(bytes, 0, bytes.Length);

                    content.WriteStream(writeStream);
                }
                else
                {
                    string header = $"--{_boundary}\r\nContent-Disposition: form-data; name=\"{content.Disposition.Name}\"\r\n\r\n";

                    var bytes = DefaultEncoding.GetBytes(header);

                    writeStream.Write(bytes, 0, bytes.Length);

                    content.WriteStream(writeStream);
                }
            }

            var footer = "\r\n--" + _boundary + "--\r\n";

            var footerbytecount = DefaultEncoding.GetByteCount(footer);

            writeStream.Write(DefaultEncoding.GetBytes(footer), 0, footerbytecount);

            writeStream.Flush();
        }

        public override void Write(WebRequest request)
        {
            if (Contents.Any())
            {
                _boundary = $"----------{Guid.NewGuid():N}";

                request.ContentType = "multipart/form-data; boundary=" + _boundary;

                using (var writeStream = request.GetRequestStream())
                {
                    WriteStream(writeStream);
                }
            }
        }
    }
}