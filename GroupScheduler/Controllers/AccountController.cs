using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GroupScheduler.Classes;
using GroupScheduler.Infrastructure.Database.Classes;
using GroupScheduler.Models;

namespace GroupScheduler.Controllers
{
    public class AccountController : SchedulerBaseController
    {
        private const string LOG_IN_ATTEMPTS = "GroupSchedulerLogInAttempts";
        private readonly IAccountDb accountDb;
        private readonly ISchedulerUserService schedulerUserService;

        public AccountController(ISchedulerContext schedulerContext, ISchedulerUserService schedulerUserService, IAccountDb accountDb)
            : base(schedulerContext)
        {
            this.accountDb = accountDb;
            this.schedulerUserService = schedulerUserService;
        }

        public ActionResult Login()
        {
            LoginModel model = new LoginModel{RedirectUrl = Request.UrlReferrer == null ? null : Request.UrlReferrer.ToString()};
            return View(model);
        }

        public ActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Get new salt
                byte[] salt = generateNewSalt();
                // Generate new hash password
                byte[] hashPassword = generateHashPassword(model.Password, salt);
                // Create the user object
                User newUser = new User
                    {
                        DisplayName = model.DisplayName,
                        Email = model.Email
                    };

                int newUserId = accountDb.RegisterNewUser(newUser, hashPassword, salt);
                // Duplicate email, return error
                if (newUserId == -1)
                {
                    this.ModelState.AddModelError("", "The emial address provided is already in use. Please provide another.");
                    return this.View(model);
                }
                newUser.UserId = newUserId;
                // Set the current Scheduler User
                schedulerUserService.SetCurrentSchedulerUser(newUser);
                schedulerUserService.SetAuthenticationCookie(newUser.Email, false);

                return this.RedirectToAction("Index", "Home");
            }
            this.ModelState.AddModelError("", "There was an error in your information. Please provide valid information.");
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            // Get the requested user's hashed password and salt from the database to verify
            dynamic logInInfo = schedulerUserService.GetSchedulerUserLogInInformation(model.Email);
            if (logInInfo == null)
            {
                this.ModelState.AddModelError("", "The email and password you provided do not match. Please try again.");
                return View(model);
            }
            byte[] salt = logInInfo.Salt;
            byte[] databaseHashedPassword = logInInfo.Password;

            // Get hashedPassword for the provided password
            byte[] providedHashedPassword = generateHashPassword(model.Password, salt);

            // Now compare both hashed passwords
            if (Convert.ToBase64String(databaseHashedPassword) == Convert.ToBase64String(providedHashedPassword))
            {
                // Set the current Scheduler User
                User loggedInUser = new User
                    {
                        UserId = logInInfo.UserId,
                        Email = model.Email,
                        DisplayName = logInInfo.DisplayName
                    };
                schedulerUserService.SetCurrentSchedulerUser(loggedInUser);
                schedulerUserService.SetAuthenticationCookie(model.Email, false);
                // If the user came from some other area in the website, redirect them back to that page
                if (model.RedirectUrl != null)
                {
                    return Redirect(model.RedirectUrl);
                }
                else
                {
                    // Otherwise this was their first page hit so bring them to the home page
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Passwords do not match
                this.ModelState.AddModelError("", "The email and password you provided do not match. Please try again.");
                return View(model);
            }
        }

        /*
         * Logs the current user out and will redirect to the homepage
         * */
        public RedirectToRouteResult Logout()
        {
            // remove all session data here
            this.schedulerUserService.Logout();
            return this.RedirectToAction("Index", "Home");
        }

        #region Private Methods

        private byte[] generateNewSalt()
        {
            byte[] salt = new byte[64];
            new Random().NextBytes(salt);
            return salt;
        }

        private byte[] generateHashPassword(string password, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainText = Encoding.UTF8.GetBytes(password);
            byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            Array.Copy(plainText, plainTextWithSaltBytes, plainText.Length);
            Array.Copy(salt, 0, plainTextWithSaltBytes, plainText.Length, salt.Length);

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        private void incrementLogInAttemps()
        {
            
        }

        #endregion

    }
}
