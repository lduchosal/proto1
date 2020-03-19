using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Proto;

namespace protoactor1
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    class ConsoleReaderActor : IActor
    {
        public const string ExitCommand = "exit";
        private readonly Logger _logger;
        private readonly PID _cwapid;
        private readonly PID _quitpid;
        private readonly PID _hellopid;

        public ConsoleReaderActor(Logger logger, PID cwapid,
            PID quitpid, PID hellopid)
        {
            _logger = logger;
            _cwapid = cwapid;
            _quitpid = quitpid;
            _hellopid = hellopid;
        }

        public Task ReceiveAsync(IContext context)
        {
            try
            {
                _logger.Debug("[ConsoleReaderActor ] ReceiveAsync");
                _logger.Debug($"[ConsoleReaderActor ] ThreadID {Thread.CurrentThread.ManagedThreadId}");
                _logger.Debug($"[ConsoleReaderActor] context ({context.GetType()})");
                _logger.Debug($"[ConsoleReaderActor] context.Message ({context.Message.GetType()})");

                var read = Console.In.ReadLine();
                if (read == "exit" || read == "quit")
                {
                    context.Send(_quitpid, new Quit());
                    return Actor.Done;
                }

                if (read.StartsWith("hello"))
                {
                    context.Send(_hellopid, new Hello { Who = read });
                }

                // send input to the console writer to process and print
                context.Send(_cwapid, read);
                context.Send(context.Self, string.Empty);
                return Actor.Done;

            }
            catch (Exception e)
            {
                Debugger.Break();
                _logger.Debug("[ConsoleReaderActor ] Exception");
                _logger.Debug(e.ToString());
            }
            return Actor.Done;

        }
    }
}