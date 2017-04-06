namespace CommonLib
{
#if NET4X
    using System;
    using System.Linq;
    using System.ServiceProcess;

    public class TimerBasedService : ServiceBase
    {
        public TimerBasedService(TimeSpan dueTime, TimeSpan period)
        {
            DueTime = dueTime;
            Period = period;
        }

        protected ITimer Timer { get; set; }
        public TimeSpan DueTime { get; }
        public TimeSpan Period { get; }
        public bool RunAsService => !Environment.UserInteractive;
        public Action ConsoleWaitAction { get; set; }

        protected override void OnStart(string[] args)
        {
            Timer.Start(DueTime, Period);
        }

        protected override void OnStop()
        {
            Timer.Stop();
        }

        public void FlexibleRun()
        {
            if (RunAsService)
            {
                Run(this);
            }
            else
            {
                OnStart(Environment.GetCommandLineArgs().Skip(1).ToArray());
                (ConsoleWaitAction ?? DefaultConsoleWaitAction)();
                OnStop();
            }
        }

        private void DefaultConsoleWaitAction()
        {
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey(true);
        }
    }
#endif
}