using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace DevLunch.Tests.Helpers
{
    public  static class ControllerTestExtensions
    {
        public static T WithAuthenticatedUser<T>(this T controller, string name, string userId) where T : Controller
        {
            var claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", name),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId)
};
            var genericIdentity = new AuthenticatedGenericIdentity(name);
            genericIdentity.AddClaims(claims);


            var genericPrincipal = new GenericPrincipal(genericIdentity, new[] { "Foo" });

            var context = new FakeHttpContext();
            context.User = genericPrincipal;

            if (controller.ControllerContext == null)
            {
                controller.ControllerContext = new ControllerContext();
            }

            controller.ControllerContext.HttpContext = context;

            return controller;
        }

        private class AuthenticatedGenericIdentity : GenericIdentity
        {
            public AuthenticatedGenericIdentity(string name) : base(name)
            {

            }

            public override bool IsAuthenticated { get { return true; } }
        }

        private class FakeHttpContext : HttpContextBase
        {
            public override IPrincipal User { get; set; }
        }
    }
}