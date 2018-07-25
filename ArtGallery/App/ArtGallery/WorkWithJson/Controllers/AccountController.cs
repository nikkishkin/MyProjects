using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt.Net;
using WorkWithJson.Auth;
using WorkWithJson.DAL;
using WorkWithJson.Models.Account;

namespace WorkWithJson.Controllers
{
    public class AccountController : ExhibitionController
    {
        public const string ControllerName = "Account";

        public const string LogInAction = "LogIn";
        public const string SignUpAction = "SignUp";
        public const string LogOutAction = "LogOut";

        [HttpGet]
        public ViewResult LogIn(int returnPageNumber)
        {
            RestoreModelState();
            return View(new LogInModel {ReturnPageNumber = returnPageNumber});
        }

        [HttpGet]
        public ViewResult SignUp(int returnPageNumber)
        {
            RestoreModelState();
            return View(new SignUpModel {ReturnPageNumber = returnPageNumber});
        }

        [HttpPost]
        public ActionResult LogIn(LogInModel logInModel)
        {
            User dbUser = null;
            if (ModelState.IsValid)
            {
                dbUser = GetContext().User
                    .FirstOrDefault(u => u.Username == logInModel.Username);
                ValidateLogInModel(dbUser, logInModel);
            }

            if (ModelState.IsValid)
            {
                Authorize(dbUser);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_LogIn");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(HomeController.IndexAction, HomeController.ControllerName,
                    new {pageNumber = logInModel.ReturnPageNumber});
            }

            SaveModelState(ModelState);
            return RedirectToAction(LogInAction, new { returnPageNumber = logInModel.ReturnPageNumber });
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

        private void Authorize(User dbUser)
        {
            // Create principal
            UserPrincipal principal = new UserPrincipal(dbUser.Username, dbUser.Id,
                dbUser.Role.Select(r => r.Name).ToArray());

            // Save it to this request and session
            string principalApplicationKey = Guid.NewGuid().ToString();
            UserPrincipal.CurrentPrincipal = principal;
            ControllerContext.HttpContext.Application[principalApplicationKey] = principal;

            // Set Forms auth cookie
            HttpCookie authCookie = GetAuthCookie(principal, principalApplicationKey);
            Response.Cookies.Add(authCookie);
        }

        private HttpCookie GetAuthCookie(UserPrincipal userPrincipal, string principalApplicationKey)
        {            
            //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
            //                                                                     userPrincipal.Identity.Name,
            //                                                                     DateTime.Now,
            //                                                                     DateTime.Now.AddMinutes(15),
            //                                                                     false, //pass here true, if you want to implement remember me functionality
            //                                                                     principalApplicationKey,
            //                                                                     FormsAuthentication.FormsCookiePath);

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

        [HttpPost]
        public ActionResult SignUp(SignUpModel signUpModel)
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
                    CreationDate = DateTime.Now
                };

                user.Role.Add(GetContext().Role.FirstOrDefault(r => r.Name == "User"));

                if (signUpModel.Username == ConfigurationManager.AppSettings["AdminName"])
                {
                    user.Role.Add(GetContext().Role.FirstOrDefault(r => r.Name == "Admin"));
                }

                GetContext().User.Add(user);
                GetContext().SaveChanges();

                user = GetContext().User
                    .FirstOrDefault(u => u.Username == signUpModel.Username);
                Authorize(user);
            }

            if (Request.IsAjaxRequest()) return PartialView("_SignUp");

            if (ModelState.IsValid)
            {
                return RedirectToAction(HomeController.IndexAction, HomeController.ControllerName,
                    new { pageNumber = signUpModel.ReturnPageNumber });
            }

            SaveModelState(ModelState);
            return RedirectToAction(SignUpAction, new { returnPageNumber = signUpModel.ReturnPageNumber });
        }

        private void ValidateSignUpModel(SignUpModel signUpModel)
        {
            User dbUser = GetContext().User
                    .FirstOrDefault(u => u.Username == signUpModel.Username);
            if (dbUser != null)
            {
                ModelState.AddModelError("Username", "Sorry! This username already exists");
            }
        }

        [HttpPost]
        public RedirectToRouteResult LogOut()
        {
            UserPrincipal.CurrentPrincipal = UserPrincipal.Empty;
            FormsAuthentication.SignOut();

            //return PartialView("_LogOut");

            return RedirectToAction(HomeController.IndexAction, HomeController.ControllerName);
        }
    }
}
