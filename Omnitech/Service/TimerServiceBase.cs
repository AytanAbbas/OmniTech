using System.Threading.Tasks;
using System;

namespace Omnitech.Service
{
    public class TimerServiceBase
    {
        private static readonly object Lock = new object();
        public async Task InvokeTimer(Func<Task> func, TimeSpan periodInterval)
        {

            var task = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        lock (Lock)
                        {
                            func().GetAwaiter().GetResult();
                        }
                    }

                    catch
                    {

                    }

                    await Task.Delay(periodInterval);

                    GC.Collect();
                }
            });
        }
    }
}
