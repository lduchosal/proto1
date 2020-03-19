using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Proto;

namespace protoactor1
{
    class HelloActor : IActor
    {
        private readonly ILogger _logger;

        public HelloActor(ILogger logger)
        {
            _logger = logger;
        }

        public Task ReceiveAsync(IContext context)
        {
            try
            {
                _logger.Debug("[HelloActor] ReceiveAsync");
                _logger.Debug($"[HelloActor ] ThreadID {Thread.CurrentThread.ManagedThreadId}");
                _logger.Debug($"[HelloActor] context ({context.GetType()})");
                _logger.Debug($"[HelloActor] context.Message ({context.Message.GetType()})");

                var msg = context.Message;
                if (msg is Hello hello)
                {
                    Console.WriteLine($"Hello {hello.Who}");
                }

                return Actor.Done;

            }
            catch (Exception e)
            {
                Debugger.Break();
                _logger.Debug("[HelloActor ] Exception");
                _logger.Debug(e.ToString());
            }

            return Actor.Done;
        }
    }
}