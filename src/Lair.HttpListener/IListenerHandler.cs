using System;
using System.Net;
using System.Threading.Tasks;

namespace Lair.HttpListener
{
    public interface IListenerHandler
    {
        Task Process(HttpListenerContext context);
        void HandleException(Exception exception);
    }
}