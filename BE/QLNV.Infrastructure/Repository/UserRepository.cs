using Dapper;
using MISA.AMIS.Core.Entities;
using MISA.AMIS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MISA.AMIS.Core.Interfaces.Infrastructures;

namespace MISA.AMIS.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        #region Field
        IMISADbContext _dbContext;
        public UserRepository(IMISADbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetUser(UserLogin userLogin)
        {
            //mã hóa pass
            //var pass_md5 = MD5Hash(userModel.Password);
            //var sql = "select * from Account where Username = @Username And Password = @Password
            var sql = "Proc_User_GetByEmail";
            var user = await _dbContext.Connection.QueryFirstOrDefaultAsync<User>(sql, userLogin);
            return user;
        }

        public async Task<User?> GetUserByToken(string token)
        {
            var sql = "Select * from User where RefreshToken = @RefreshToken";
            var param = new DynamicParameters();
            param.Add("@RefreshToken", token);
            var res = await _dbContext.Connection.QueryFirstOrDefaultAsync<User>(sql, param);
            return res;
        }

        public string MD5Hash(string password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public async Task<int> UpdateAsync(User user, Guid userId)
        {
            var res = await _dbContext.UpdateAsync<User>(user, userId);
            return res;
        }
        #endregion
    }
}
