using Omnitech.Dal.AdoNet;
using Omnitech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omnitech.Service
{
    public class BaseService
    {
        private readonly UserMenuManager _userMenuManager;
        private readonly Tps575UrlService _tps575UrlService;

        public BaseService(UserMenuManager userMenuManager, Tps575UrlService tps575UrlService)
        {
            _userMenuManager = userMenuManager;
            _tps575UrlService = tps575UrlService;
        }

        public async Task<List<Submenu>> GetSubmenusByUserIdAsync(int userId)
        {
            return await _userMenuManager.GetSubmenusByUserIdAsync(userId);
        }

        public async Task<List<Submenu>> GetSubmenusByUsernameAsync(string username)
        {
            return await _userMenuManager.GetSubmenusByUsernameAsync(username);
        }

        public async Task<Tps575Url> GetUrlByUsernameAsync(string username)
        {
            return await _tps575UrlService.GetUrlByUsernameAsync(username);
        }
    }
}
