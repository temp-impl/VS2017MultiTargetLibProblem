using System;

namespace CommonLib
{
    public interface ITimer
    {
        TimerStatus Status { get; }

        void Start(TimeSpan dueTime, TimeSpan period);

        void Stop();
    }

    public enum TimerStatus : int
    {
        Stopped = 0,
        Stopping = 1,
        Started = 2
    }

    public static class ITimerExtensions
    {
        public static void Start(this ITimer timer, TimeSpan period)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            timer.Start(TimeSpan.Zero, period);
        }
    }
}