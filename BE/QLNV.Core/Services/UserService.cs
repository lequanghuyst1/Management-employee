using MISA.AMIS.Core.Entities;
using MISA.AMIS.Core.Exceptions;
using MISA.AMIS.Core.Interfaces.Infrastructures;
using MISA.AMIS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        Dictionary<string, string[]>? erorrs = new Dictionary<string, string[]>();
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> LoginAsync(UserLogin userLogin)
        {
            var user = await _userRepository.GetUser(userLogin);
            if(user == null)
            {
                erorrs?.Add("Account", new string[] { Resources.ResourceVN.AccountNotCorrect });
                throw new ValidateException(System.Net.HttpStatusCode.BadRequest, Resources.ResourceVN.AccountNotCorrect, erorrs);
            }
            return user;
        }

        public async Task<int> UpdateServiceAsyns(User user, Guid userId)
        {
            var res = await _userRepository.UpdateAsync(user, userId);
            return res;
        }
    }
}
