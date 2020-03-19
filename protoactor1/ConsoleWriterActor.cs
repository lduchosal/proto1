using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Proto;

namespace protoactor1
{
    class ConsoleWriterActor : IActor
    {
        private readonly ILogger _logger;

        public ConsoleWriterActor(ILogger logger)
        {
            _logger = logger;
        }

        public Task ReceiveAsync(IContext context)
        {
            try
            {
                _logger.Debug("[ConsoleWriterActor] ReceiveAsync");
                _logger.Debug($"[ConsoleWriterActor ] ThreadID {Thread.CurrentThread.ManagedThreadId}");
                _logger.Debug($"[ConsoleWriterActor] context ({context.GetType()})");
                _logger.Debug($"[ConsoleWriterActor] context.Message ({context.Message.GetType()})");

                if (context.Message is Started)
                {
                    _logger.Debug($"[ConsoleWriterActor] is started");

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Please provide an input.\n");
                    Console.ResetColor();
                    return Actor.Done;
                }

                if (context.Message is string msg)
                {

                    // make sure we got a message
                    if (string.IsNullOrEmpty(msg))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Please provide an input.\n");
                        Console.ResetColor();
                        return Actor.Done;
                    }

                    // if message has even # characters, display in red; else, green
                    var even = msg.Length % 2 == 0;
                    var color = even ? ConsoleColor.Red : ConsoleColor.Green;
                    var alert = even
                        ? "Your string had an even # of characters.\n"
                        : "Your string had an odd # of characters.\n";
                    Console.ForegroundColor = color;
                    Console.WriteLine(alert);
                    Console.ResetColor();

                    return Actor.Done;
                }

                return Actor.Done;

            }
            catch (Exception e)
            {
                Debugger.Break();
                _logger.Debug("[ConsoleWriterActor ] Exception");
                _logger.Debug(e.ToString());
            }

            return Actor.Done;
        }
    }
}