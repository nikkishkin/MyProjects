using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt.Net;
using TaskOperator.Core;
using TaskOperator.Entities;
using TaskOperator.Logic.Interfaces;
using TaskOperator.Logic.Services;
using TaskOperator.Web.Models.Account;

namespace TaskOperator.Web.Controllers
{
    public class AccountController : TaskOperatorController
    {
        public const string ControllerName = "Account";

        public const string LogInAction = "LogIn";
        public const string SignUpAction = "SignUp";
        public const string LogOutAction = "LogOut";

        private const string ManagerName = "nik";

        private readonly IUserBlo _userBlo;

        public AccountController(IUserBlo userBlo)
        {
            _userBlo = userBlo;
        }

        [HttpPost]
        public RedirectToRouteResult LogOut()
        {
            UserPrincipal.CurrentPrincipal = UserPrincipal.Empty;
            FormsAuthentication.SignOut();

            //return PartialView("_LogOut");

            return RedirectToAction(HomeController.IndexAction, HomeController.ControllerName);
        }

        [HttpPost]
        public PartialViewResult LogIn(LogInModel logInModel)
        {
            User dbUser = null;
            if (ModelState.IsValid)
            {
                dbUser = _userBlo.GetUser(logInModel.Username);
                ValidateLogInModel(dbUser, logInModel);
            }

            if (ModelState.IsValid)
            {
                Authorize(dbUser);
            }

            return PartialView("_LogIn");
        }

        [HttpPost]
        public PartialViewResult SignUp(SignUpModel signUpModel)
        {
            if (ModelState.IsValid)
            {
                ValidateSignUpModel(signUpModel);
            }

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = signUpModel.Email,
                    First_Name = signUpModel.FirstName,
                    Last_Name = signUpModel.LastName,
                    Username = signUpModel.Username,
                    Password = BCrypt.Net.BCrypt.HashString(signUpModel.Password),
                    IsManager = signUpModel.Username == ManagerName
                };

                _userBlo.AddUser(user);

                Authorize(user);
            }

            return PartialView("_SignUp");
        }

        // Verify that user exists and password is right
        private void ValidateLogInModel(User dbUser, LogInModel logInModel)
        {
            if (dbUser == null)
            {
                ModelState.AddModelError("Username", "This username doesn't exist");
            }
            else
            {
                try
                {
                    if (!BCrypt.Net.BCrypt.Verify(logInModel.Password, dbUser.Password))
                    {
                        ModelState.AddModelError("Password", "Password is wrong!");
                    }
                }
                catch (SaltParseException)
                {
                    ModelState.AddModelError("Password", "Password is wrong!");
                }
            }
        }

        private void ValidateSignUpModel(SignUpModel signUpModel)
        {
            if (_userBlo.UserExists(signUpModel.Username))
            {
                ModelState.AddModelError("Username", "Sorry! This username already exists");
            }
        }

        private void Authorize(User dbUser)
        {
            // Create principal
            UserPrincipal userPrincipal = new UserPrincipal(dbUser.Username, dbUser.Id, dbUser.IsManager);

            // Save it to this request and session
            string principalApplicationKey = Guid.NewGuid().ToString();
            UserPrincipal.CurrentPrincipal = userPrincipal;
            ControllerContext.HttpContext.User = userPrincipal;
            ControllerContext.HttpContext.Application[principalApplicationKey] = userPrincipal;

            // Set Forms auth cookie
            HttpCookie authCookie = GetAuthCookie(userPrincipal, principalApplicationKey);
            Response.Cookies.Add(authCookie);
        }

        private HttpCookie GetAuthCookie(UserPrincipal userPrincipal, string principalApplicationKey)
        {
            HttpCookie initialCookie = FormsAuthentication.GetAuthCookie(userPrincipal.Identity.Name, false);
            FormsAuthenticationTicket initialTicket = FormsAuthentication.Decrypt(initialCookie.Value);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, // version
                                                                                 userPrincipal.Identity.Name, // username
                                                                                 initialTicket.IssueDate, // issue date
                                                                                 initialTicket.Expiration, // expiration
                                                                                 false, // persistance
                                                                                 principalApplicationKey, // user data
                                                                                 FormsAuthentication.FormsCookiePath);
            // Build auth cookie
            return new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
        }
    }
}