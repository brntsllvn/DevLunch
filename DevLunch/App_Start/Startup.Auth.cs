using System;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.GitHub;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DevLunch
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        OnValidateIdentity =
            //           SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(TimeSpan.FromMinutes(30),
            //               (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});
            // app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
               // LoginPath = new PathString("/Account/Login"),
                //Provider = new CookieAuthenticationProvider
                //{
                //    // Enables the application to validate the security stamp when the user logs in.
                //    // This is a security feature which is used when you change a password or add an external login to your account.  
                //    OnValidateIdentity =
                //         SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(TimeSpan.FromMinutes(30),
                //             (manager, user) => user.GenerateUserIdentityAsync(manager))
                //}
            });
            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);

            app.UseGitHubAuthentication(
                clientId: "40115333d4281ebe899a",
                clientSecret: "b6ccafcc7cb27c3adcf186b91f0f5fdf399d857a");
        }
    }
}