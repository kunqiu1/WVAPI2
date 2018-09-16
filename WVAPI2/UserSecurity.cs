using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WVAPIDbEF;
namespace WVAPI2
{
    public class UserSecurity
    {
        public static bool Login(string username , string password)
        {
            using (wvDB entities =new wvDB())
            {
                return entities.Users.Any(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && user.Password == password);
            }
        }
    }
}