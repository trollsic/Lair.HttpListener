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
            asyncHttpListener.Initialize(new string[] { "http://+:8082" }, 5000);
            asyncHttpListener.Start();

            Console.WriteLine("Listening...");
            Console.ReadLine();
        }
    }
}
