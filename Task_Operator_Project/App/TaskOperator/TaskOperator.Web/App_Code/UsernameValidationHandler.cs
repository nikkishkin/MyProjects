using System.Web;
using System.Web.Mvc;
using TaskOperator.Logic.Interfaces;

namespace TaskOperator.Web
{
    public class UsernameValidationHandler: IHttpHandler
    {
        private readonly IUserBlo _userBlo ;

        //public UsernameValidationHandler(IUserBlo userBlo)
        //{
        //    _userBlo = userBlo;
        //}

        public UsernameValidationHandler()
        {
            _userBlo = DependencyResolver.Current.GetService<IUserBlo>();
        }

        public void ProcessRequest(HttpContext context)
        {
            if (_userBlo.UserExists(context.Request["username"]))
            {
                // Username is invalid
                context.Response.Write("0");
            }
            else
            {
                // Username is valid
                context.Response.Write("1");
            }
        }

        public bool IsReusable 
        {
            get { return false; }
        }
    }
}