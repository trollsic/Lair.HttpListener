# Lair.HttpListener
Simple C# HttpListener Async implementation.

Just raw HTTP processing, BYOL (Bring Your Own Logic).

**Note:** opening listener on port is requires Administrator priviliges, be sure to run VS or your app As Administrator


##Usage
All boils down to creating a handler to process the requests, giving the handler to HttpListener, initialize and start.

Create handler class that implements IListenerHandler interface
```C#
public interface IListenerHandler
{
    Task Process(HttpListenerContext context);
    void HandleException(Exception exception);
}
```

Example of RawHttpHandler.cs from Lair.HttpListenerConsole source
```C#
public class RawHttpHandler : IListenerHandler
{
    public Task Process(HttpListenerContext context)
    {
        Stream outputStream = context.Response.OutputStream;
        
        string resp = "Goodbye world!";
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
```

And then the main part to run it:
```C#
var rawHttpHandler = new RawHttpHandler();
AsyncHttpListener asyncHttpListener = new AsyncHttpListener(rawHttpHandler);
asyncHttpListener.Initialize(prefixes: new string[] { "http://+:8082" }, requestQueueLength: 5000);
asyncHttpListener.Start();
```

Initialize method of AsyncHttpListener receives hostname and port prefixes that listener will be bounded to.
Second optional parameter "requestQueueLength" can be used to set Max Requests for listener.
If no value passed, HttpListener will be using the system's default, which is 1000.
To check the current settings values, you can use the following command:
`netsh http show servicestate`

AsyncHttpListener lifetime should be same as your app while it's running.
