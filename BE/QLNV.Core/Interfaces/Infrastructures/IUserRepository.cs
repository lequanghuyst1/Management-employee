using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Infrastructures
{
    public interface IUserRepository
    {
        /// <summary>
        /// Lấy ra tài khoản đăng nhập hợp lệ
        /// </summary>
        /// <param name="user">thông tin đăng nhập</param>
        /// <returns>Thông tin tài khoản đăng nhập</returns>
        /// Created By: LQHUY(18/02/2024)
        Task<User?> GetUser(UserLogin userLogin);

        Task<int> UpdateAsync(User user, Guid userId);

        Task<User?> GetUserByToken(string token);

    }
}
