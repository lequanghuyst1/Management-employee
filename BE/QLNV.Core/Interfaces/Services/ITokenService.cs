using MISA.AMIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Interfaces.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Hàm đăng nhập vào hệ thống
        /// </summary>
        /// <param name="user">thông tin đăng nhập</param>
        /// <returns>
        /// Thông tin của token gồm access token và refreshToken
        /// </returns
        /// Created By: LQHUY(20/02/2024)
        Task<TokenModel> Login(User user);

        /// <summary>
        /// Hàm lấy ra chuỗi refreshToken
        /// </summary>
        /// <returns>chuỗi refreshToken</returns>
        /// Created By: LQHUY(20/02/2024)
        string GenerateRefreshToken();

        /// <summary>
        /// Hàm lấy ra thông tin token mới khi token cũ hết hạn
        /// </summary>
        /// <param name="tokenModel">Thông tin token cũ</param>
        /// <returns>
        /// Thông tin của token mới gồm access token và refreshToken
        /// </returns
        /// Created By: LQHUY(20/02/2024)
        Task<TokenModel> RenewToken(TokenModel tokenModel);
    }
}
