using System;
using System.IO;
using System.Net;

namespace Jal.HttpClient.Model
{
    public class HttpStreamContent : HttpContent
    {
        public Stream Content { get; set; }

        public long BufferLenght { get; set; }

        public HttpStreamContent(Stream content, long bufferlenght= 4096)
        {
            BufferLenght = bufferlenght;
            Content = content;
        }

        public override string ToString()
        {
            return "Stream";
        }

        public override void Write(WebRequest request)
        {
            if (Content != null)
            {
                WriteContentType();

                using (var writeStream = request.GetRequestStream())
                {
                    WriteStream(writeStream);
                }
            }
        }

        public override void WriteStream(Stream writeStream)
        {
            var buffer = new byte[checked(Math.Min(BufferLenght, Content.Length))];

            int bytesread;

            while ((bytesread = Content.Read(buffer, 0, buffer.Length)) != 0)
            {
                writeStream.Write(buffer, 0, bytesread);
            }

            writeStream.Flush();
        }
    }
}