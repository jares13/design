using System;
using System.Linq;
using System.Timers;

namespace PerfLogger
{
	class Program
	{
		static void Main(string[] args)
		{
			
			var sum = 0.0;
			using (PerfLogger.Measure(t => Console.WriteLine("for: {0}", t)))
				for (var i = 0; i < 100000000; i++) sum += i;
			using (PerfLogger.Measure(t => Console.WriteLine("linq: {0}", t)))
				sum -= Enumerable.Range(0, 100000000).Sum(i => (double)i);
			Console.WriteLine(sum);
			
		}
	}

    class PerfLogger : IDisposable
    {

        private long Timer;
        private Action<long> shower;

        public PerfLogger(long timer, Action<long> shower)
        {
            this.Timer = timer;
            this.shower = shower;
        }

        static public IDisposable Measure(Action<long> shower)
        {

            var startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return new PerfLogger(startTime, shower);

        }

        public void Dispose()
        {
            Timer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - Timer;
            shower(Timer);
        }
    }

}
