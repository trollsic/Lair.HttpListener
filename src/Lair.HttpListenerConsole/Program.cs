using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lair.HttpListener;

namespace Lair.HttpListenerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawHttpHandler = new RawHttpHandler();
            AsyncHttpListener asyncHttpListener = new Lair.HttpListener.AsyncHttpListener(rawHttpHandler);
            asyncHttpListener.Initialize(new string[] { "http://+:80/", "https://+:443/" }, 5000);
            asyncHttpListener.Start();

            //netsh http add sslcert ipport=0.0.0.0:8083 certhash=CERT_THUMBPRINT appid={GUID}

            Console.WriteLine("Listening...");
            Console.ReadLine();
        }
    }
}
