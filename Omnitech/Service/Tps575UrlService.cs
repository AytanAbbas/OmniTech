using Omnitech.Dal.AdoNet;
using Omnitech.Models;
using Omnitech.Utilities;
using System.Threading.Tasks;

namespace Omnitech.Service
{
    public class Tps575UrlService
    {
        private readonly Tps575UrlRepository _tps575UrlRepository;
        public Tps575UrlService(Tps575UrlRepository tps575UrlRepository)
        {
            _tps575UrlRepository = tps575UrlRepository;

            Enums.Tps575Url = tps575UrlRepository.GetActiveUrlAsync().GetAwaiter().GetResult();
        }

        public async Task<Tps575Url> GetUrlByUsernameAsync(string username)
        {
            return await _tps575UrlRepository.GetUrlByUsernameAsync(username);
        }
    }
}
