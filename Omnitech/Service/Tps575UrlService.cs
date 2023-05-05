using Omnitech.Dal.AdoNet;
using Omnitech.Utilities;

namespace Omnitech.Service
{
    public class Tps575UrlService
    {
        public Tps575UrlService(Tps575UrlRepository tps575UrlRepository)
        {
            Enums.Tps575Url = tps575UrlRepository.GetActiveUrlAsync().GetAwaiter().GetResult();
        }
    }
}
