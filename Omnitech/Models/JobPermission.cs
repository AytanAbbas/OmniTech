using System;

namespace Omnitech.Models
{
    public class JobPermission
    {
        public int Id { get; set; }
        public int LineNo { get; set; }
        public string JobName { get; set; }
        public int HasPermission { get; set; }
        public int IsRunning { get; set; }
        public DateTime LastRun { get; set; }
        public DateTime NextRun { get; set; }
        public int IntervalUnit { get; set; }
        public string IntervalType { get; set; }
        public int DayCount { get; set; }
        public int DateBetween { get; set; }
    }
}
