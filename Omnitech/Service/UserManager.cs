using Omnitech.Dal.AdoNet;
using Omnitech.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omnitech.Service
{
    public class UserManager
    {
        private readonly UserRepository _userDal;

        public UserManager(UserRepository userDal)
        {
            _userDal = userDal;
        }

        public async Task AddAsync(User user)
        {
            await _userDal.AddAsync(user);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userDal.GetByUsernameAsync(username);
        }

        public async Task<List<OperationClaim>> GetClaimsAsync(User user)
        {
            return await _userDal.GetClaimsAsync(user);
        }
    }
}
