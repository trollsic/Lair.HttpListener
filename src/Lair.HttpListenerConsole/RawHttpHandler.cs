using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lair.HttpListener;

namespace Lair.HttpListenerConsole
{
    public class RawHttpHandler : IListenerHandler
    {
        public Task Process(HttpListenerContext context)
        {
            Stream outputStream = context.Response.OutputStream;
            
            string resp = "OK";
            byte[] buffer = Encoding.UTF8.GetBytes(resp);
            outputStream.Write(buffer, 0, buffer.Length);
            outputStream.Close();
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";

            context.Response.Close();

            return Task.FromResult(true);
        }

        public void HandleException(Exception exception)
        {
            Console.Error.WriteLine(exception);
        }
    }
}
