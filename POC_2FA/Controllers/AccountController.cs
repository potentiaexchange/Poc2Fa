using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProvaDeConceito2FA.Models;
using Google.Authenticator;
using System.Configuration;
using Tarczynski.NtpDateTime;

namespace ProvaDeConceito2FA.Controllers
{
    public class AccountController : Controller
    {
        private const string emailTest1 = "test01@gmail.com";
        private const string passwordTest1 = "test01";
        private const string emailTest2 = "test02@gmail.com";
        private const string passwordTest2 = "test02";
        private readonly string key = ConfigurationManager.AppSettings.Get("KeyGenerate2FA").ToString();

        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Login(string email, string password)
        {
            HttpContext context = System.Web.HttpContext.Current;

            if (CredentialIsValid(email, password))
            {
                if (email == emailTest1 && password == passwordTest1)
                {
                    context.Session["user"] = "test1";                   
                }
                else if (email == emailTest2 && password == passwordTest2)
                {
                    context.Session["user"] = "test2";
                }
                return View("Home");
            }
            
            ViewBag.Message = "Invalid Credentials";
            return View();
        }

        private bool CredentialIsValid(string email, string password)
        {
            return email == emailTest1 && password == passwordTest1 || email == emailTest2 && password == passwordTest2;
        }

        public ActionResult Enable2FA()
        {
            HttpContext context = System.Web.HttpContext.Current;
            var user = context.Session["user"];

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("Test2FA", user.ToString() + "@test.com", key + user.ToString(), 300, 300);
            ViewBag.QrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;     
            ViewBag.ManualEntryKey= setupInfo.ManualEntryKey;

            return View();
        }

        public ActionResult VerifyCode()
        {
            return View();
        }

        public ActionResult Verify(string code)
        {
            HttpContext context = System.Web.HttpContext.Current;

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string currentPin = tfa.GetCurrentPIN(key + context.Session["user"].ToString(), DateTime.UtcNow.FromNtp().AddHours(DateTime.UtcNow.Hour - DateTime.Now.Hour));
            if (currentPin.Equals(code))
            {
                ViewBag.Message = "Success";
            }
            else
            {
                ViewBag.Message = $"Failure current key used is {currentPin}";
            }

            return View("VerifyCode");
        }
    }
}