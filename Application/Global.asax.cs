using Newtonsoft.Json;
using Pharma.Models;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Pharma
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                HttpCookie authCookie = Request.Cookies["pharmaappCookie"];//HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName]; //
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    var serializeModel = JsonConvert.DeserializeObject<CSerializeModel>(authTicket.UserData);

                    FormsIdentity formsIdentity = new FormsIdentity(authTicket);

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(formsIdentity);

                    //var user = this.UserService.GetUserByEmail(ticket.Name);

                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, authTicket.Name));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, serializeModel.FirstName + " " + serializeModel.LastName));
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, serializeModel.UserId.ToString()));
                    foreach (var role in serializeModel.RoleName.ToArray())
                    {
                        claimsIdentity.AddClaim(
                            new Claim(ClaimTypes.Role, role));
                    }
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    //CPrincipal principal = new CPrincipal(authTicket.Name);

                    //principal.UserId = serializeModel.UserId;
                    //principal.FirstName = serializeModel.FirstName;
                    //principal.LastName = serializeModel.LastName;
                    //principal.Roles = serializeModel.RoleName.ToArray<string>();

                    HttpContext.Current.User = claimsPrincipal;
                }

            }
            catch (Exception)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}
