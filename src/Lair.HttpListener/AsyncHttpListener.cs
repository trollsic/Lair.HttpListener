using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Lair.HttpListener
{
    public class AsyncHttpListener
    {
        private readonly IListenerHandler handler;
        private System.Net.HttpListener listener;
        private SemaphoreSlim semaphore;
        private bool shouldStop = false;
        private int requestQueueLength = 0;

        public AsyncHttpListener(IListenerHandler handler)
        {
            this.handler = handler;

            this.semaphore = new SemaphoreSlim(Environment.ProcessorCount * 2, Environment.ProcessorCount * 2);
        }

        public void Initialize(string[] prefixes, int requestQueueLength = 0)
        {
            this.listener = new System.Net.HttpListener();

            foreach (string prefix in prefixes)
            {
                if (!prefix.EndsWith("/"))
                {
                    this.listener.Prefixes.Add(prefix + "/");
                }
                else
                {
                    this.listener.Prefixes.Add(prefix);
                }
            }

            if (requestQueueLength > 0)
            {
                this.requestQueueLength = requestQueueLength;
            }
        }

        public void Start()
        {
            this.listener.Start();
            
            HttpApi.SetRequestQueueLength(this.listener, this.requestQueueLength);

            Run().ConfigureAwait(false);
        }

        private async Task Run()
        {
            while (true && !this.shouldStop)
            {
                await semaphore.WaitAsync();

                try
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                    listener.GetContextAsync().ContinueWith(async ctx =>
                    {
                        semaphore.Release();
                        HttpListenerContext context = await ctx;

                        await this.handler.Process(context);
                    }).ConfigureAwait(continueOnCapturedContext: false);

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
                catch (Exception ex)
                {
                    this.handler.HandleException(ex);
                }
            }
        }

        public void Stop()
        {
            this.shouldStop = true;
            this.listener.Stop();
        }
    }
}
