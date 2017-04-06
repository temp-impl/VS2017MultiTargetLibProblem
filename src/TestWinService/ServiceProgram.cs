using CommonLib;
using System;
using System.Threading;

namespace TestWinService
{
    public class ServiceProgram : TimerBasedService
    {
        public static ServiceProgram Context { get; } = new ServiceProgram();

        public static void Main(string[] args)
        {
            using (Context) Context.FlexibleRun();
        }

        public ServiceProgram() : base(TimeSpan.Zero, TimeSpan.FromSeconds(5))
        {

            Timer = new FixedDelayTimer(() => {

                Console.WriteLine("windows service running...");

            }, Timeout.InfiniteTimeSpan);
        }
    }
}