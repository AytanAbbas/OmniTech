using System.Threading.Tasks;
using System;

namespace Omnitech.Service
{
    public class TimerServiceBase
    {
        public async Task InvokeTimer(Func<Task> func, TimeSpan periodInterval)
        {
            var task = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await func();
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
