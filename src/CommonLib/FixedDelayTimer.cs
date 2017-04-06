using System;
using System.Threading;

namespace CommonLib
{
    public class FixedDelayTimer : ITimer
    {
        public FixedDelayTimer(Action callback, TimeSpan stopTimeout = default(TimeSpan))
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            _callback = callback;
            _stopTimeout = stopTimeout;
        }
        private readonly Action _callback;
        private readonly TimeSpan _stopTimeout;
        private Timer _timer;
        private readonly ManualResetEvent _runningHandle = new ManualResetEvent(false);
        private readonly object _instanceLock = new object();
        public TimerStatus Status { get; private set; } = TimerStatus.Stopped;

        private TimerCallback CreateCallback(TimeSpan period)
        {
            return state =>
            {
                _runningHandle.Reset();
                _callback();
                _runningHandle.Set();
                lock (_instanceLock) _timer?.Change(period, Timeout.InfiniteTimeSpan);
            };
        }

        public void Start(TimeSpan dueTime, TimeSpan period)
        {
            lock (_instanceLock)
            {
                Status = TimerStatus.Started;
                _timer?.Dispose();
                _timer = new Timer(CreateCallback(period), null, dueTime, Timeout.InfiniteTimeSpan);
            }
        }

        public void Stop()
        {
            Status = TimerStatus.Stopping;
            lock (_instanceLock)
            {
                if (_timer != null)
                {
                    if (_stopTimeout != TimeSpan.Zero) _runningHandle.WaitOne(_stopTimeout);
                    _timer.Dispose();
                    _timer = null;
                    Status = TimerStatus.Stopped;
                }
            }
        }
    }
}