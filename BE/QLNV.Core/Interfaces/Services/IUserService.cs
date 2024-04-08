using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Kiếm tra xem tài khoản đăng nhập có hợp lệ hay không
        /// </summary>
        /// <param name="userLogin">thông tin đăng nhập</param>
        /// <returns>
        /// Thông tin tài khoản hợp lệ
        /// </returns>
        /// <exception cref="ValidateException"></exception>
        Task<User> LoginAsync(UserLogin userLogin);

        Task<int> UpdateServiceAsyns(User user, Guid userId);
    }
}
