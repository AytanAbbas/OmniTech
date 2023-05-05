using System;

namespace Omnitech.Service
{
    public class PrintTimerService : TimerServiceBase
    {
        public PrintTimerService(PrintService service)
        {
            InvokeTimer(service.PrintAllAsync, TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
        }
    }
}
