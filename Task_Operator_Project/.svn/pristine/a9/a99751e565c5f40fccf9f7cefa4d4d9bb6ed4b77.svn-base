using System;
using System.Security.Principal;
using System.Threading;

namespace TaskOperator.Core
{
    public class UserPrincipal : GenericPrincipal
    {
        public static UserPrincipal Empty = new UserPrincipal(String.Empty, 0, false);

        public static UserPrincipal CurrentPrincipal
        {
            get { return (UserPrincipal)Thread.CurrentPrincipal; }
            set { Thread.CurrentPrincipal = value; }
        }

        public UserPrincipal(string userName, int userId, bool isManager)
            : this(new GenericIdentity(userName), new string[] { })
        {
            UserId = userId;
            IsManager = isManager;
        }

        public UserPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
        }

        public int UserId { get; set; }

        public bool IsManager { get; set; }
    }
}
