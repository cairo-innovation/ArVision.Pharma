using Newtonsoft.Json;
using Pharma.membership;
using Pharma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pharma.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            ViewBag.ReturnUrl = returnUrl;
            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                var user = (CmembershipUser)Membership.GetUser(model.UserName, false);
                if (user != null)
                {
                    CSerializeModel userModel = new CSerializeModel()
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RoleName = user.Roles.Select(r => r.RoleName).ToList()
                    };

                    string userData = JsonConvert.SerializeObject(userModel);
                    //Create a new Forms Authentication Ticket
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                    (
                    1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                        );

                    ////Create the FormsIdentity object
                    FormsIdentity formsIdentity = new FormsIdentity(authTicket);
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(formsIdentity);

                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, authTicket.Name));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, userModel.FirstName + " " + userModel.LastName));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, userModel.UserId.ToString()));
                    foreach (var role in userModel.RoleName.ToArray<string>())
                    {
                        claimsIdentity.AddClaim(
                            new Claim(ClaimTypes.Role, role));
                    }
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.User = claimsPrincipal;
                    string enTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie("pharmaappCookie", enTicket);
                    faCookie.Expires = DateTime.Now.AddMinutes(15);
                    Response.Cookies.Add(faCookie);
                    if (returnUrl == null || returnUrl == "" || returnUrl == "/")
                    {
                        returnUrl = "/visits/details/";
                    }
                    return Redirect(returnUrl);
                }
            }
            //var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }
    }
}