using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleLock1
{
    class Program
    {
        static void Main(string[] args)
        {

            bool.TryParse(args.FirstOrDefault(), out bool triggerLockbug);
            if (triggerLockbug)
            {
                Console.WriteLine(@"
1. Console.WriteLine 
2. Set Console.ForegroundColor on different thread 
3. This (often) locks the console
4. Console.ReadLine is locked and never return

Switch triggerLockbug to false to see whats is indeed excepted 
Note : Try multiple time to reproduce the problem ..

--

SDK .NET Core (reflétant tous les global.json) :
 Version:   3.1.200
 Commit:    c5123d973b

Environnement d'exécution :
 OS Name:     Mac OS X
 OS Version:  10.15
 OS Platform: Darwin
 RID:         osx.10.15-x64
 Base Path:   /usr/local/share/dotnet/sdk/3.1.200/

Host (useful for support):
  Version: 3.1.2
  Commit:  916b5cba26

.NET Core SDKs installed:
  3.1.200 [/usr/local/share/dotnet/sdk]

.NET Core runtimes installed:
  Microsoft.AspNetCore.App 3.1.2 [/usr/local/share/dotnet/shared/Microsoft.AspNetCore.App]
  Microsoft.NETCore.App 3.1.2 [/usr/local/share/dotnet/shared/Microsoft.NETCore.App]

To install additional .NET Core runtimes or SDKs:
  https://aka.ms/dotnet-download


");
                Console.WriteLine($"Thread : {Thread.CurrentThread.ManagedThreadId}");
            }

            var wait1 = new AutoResetEvent(false);
            var wait2 = new AutoResetEvent(false);
            
            Task.Factory.StartNew(() =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Inside thread");
                Console.ResetColor();
                
                wait1.Set();
            });

            Task.Factory.StartNew(() =>
            {
                var line = Console.ReadLine();
                wait2.Set();
            });

            WaitHandle.WaitAll(new[] {wait1, wait2});
        }
    }
}