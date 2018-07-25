using System;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace WorkWithJson.Auth
{
    public class UserPrincipal : GenericPrincipal
    {
        public static UserPrincipal Empty = new UserPrincipal(String.Empty, 0, new string[]{});

        public static UserPrincipal CurrentPrincipal
        {
            get { return (UserPrincipal)Thread.CurrentPrincipal; }
            set
            {
                Thread.CurrentPrincipal = value;
                HttpContext.Current.User = value;
            }
        }

        public UserPrincipal(string userName, int userId, string[] roles)
            : base(new GenericIdentity(userName), roles)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}