using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RefreshToken{ get; set; }
        public DateTime RefreshTRefreshTokenExpiryTime { get; set; }
        public DateTime AccessTokenExpiryTime { get; set; }
    }
}
