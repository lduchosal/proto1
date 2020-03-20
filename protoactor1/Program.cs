using System;
using System.Text;
using System.Threading;
using Proto;

namespace protoactor1
{
    public class Program
    {
        
        public static void Main(string[] argv)
        {
            var logger = new Logger();

            var quitev = new AutoResetEvent(false);
            logger.Debug($"[Program ] ThreadID {Thread.CurrentThread.ManagedThreadId}");
            
            var rootContext = new RootContext();
            var hello = Props.FromProducer(() => new HelloActor(logger));
            var write = Props.FromProducer(() => new ConsoleWriterActor(logger));
            var quit = Props.FromProducer(() => new QuitActor(logger, quitev));

            var hellopid = rootContext.Spawn(hello);
            var writepid = rootContext.Spawn(write);
            var quitpid = rootContext.Spawn(quit);

            var read = Props.FromProducer(() => new ConsoleReaderActor(logger, writepid, quitpid, hellopid));
            var readpid = rootContext.Spawn(read);


            rootContext.Send(hellopid, new Hello
            {
                Who = "Coucou"
            });

            logger.Debug("Waiting...");
            quitev.WaitOne();
        }
    }

    interface ILogger
    {
        void Debug(string msg);
    }
    class Logger : ILogger
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public void Debug(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}