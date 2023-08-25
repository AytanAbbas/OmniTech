using Omnitech.Models;

namespace Omnitech.Utilities
{
    public class Enums
    {
        public enum JobsPermission
        {
            PharmacyPrintService = 34,
            KNPrintService = 35,
        }

        public static Tps575Url Tps575Url { get; set; } = null;
        public static Tps575Url Tps575UserUrl { get; set; } = null;

    }
}
