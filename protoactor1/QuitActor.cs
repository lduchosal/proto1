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
    class QuitActor : IActor
    {
        public const string ExitCommand = "exit";
        private readonly Logger _logger;
        private readonly AutoResetEvent _quitev;

        public QuitActor(Logger logger, AutoResetEvent quitev)
        {
            _logger = logger;
            _quitev = quitev;
        }

        public Task ReceiveAsync(IContext context)
        {
            try
            {

                _logger.Debug("[QuitActor ] ReceiveAsync");
                _logger.Debug($"[QuitActor ] ThreadID {Thread.CurrentThread.ManagedThreadId}");
                _logger.Debug($"[QuitActor] context ({context.GetType()})");
                _logger.Debug($"[QuitActor] context.Message ({context.Message.GetType()})");

                if (context.Message is Quit)
                {
                    // shut down the system (acquire handle to system via
                    // this actors context)
                    //context.();
                    _quitev.Set();
                    return Actor.Done;
                }

                return Actor.Done;

            }
            catch (Exception e)
            {
                Debugger.Break();
                _logger.Debug("[QuitActor ] Exception");
                _logger.Debug(e.ToString());
            }


            return Actor.Done;
        }
    }
}