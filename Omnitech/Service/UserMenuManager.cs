using Omnitech.Dal.AdoNet;
using Omnitech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omnitech.Service
{
    public class UserMenuManager
    {
        private readonly UserMenuRepository _userMenuRepository;

        public UserMenuManager(UserMenuRepository userMenuRepository)
        {
            _userMenuRepository = userMenuRepository;
        }

        public async Task<List<Submenu>> GetSubmenusByUserIdAsync(int userId)
        {
            return await _userMenuRepository.GetSubmenusByUserIdAsync(userId);
        }

        public async Task<List<Submenu>> GetSubmenusByUsernameAsync(string username)
        {
            return await _userMenuRepository.GetSubmenusByUsernameAsync(username);
        }
    }
}

