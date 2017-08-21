namespace Procons.Durrah.Providers
{
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Procons.Durrah.Auth;
    using Procons.Durrah.Common;

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            ApplicationUser user = null; ;
            try
            {
              
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
             catch(Exception ex)
            {

            }

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "User did not confirm email.");
                return;
            }

            //ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            ClaimsIdentity oAuthIdentity = userManager.CreateIdentityAsync(user, "JWT").Result;
            oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
            oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));

            List<Claim> roles = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            AuthenticationProperties properties = CreateProperties(user.UserName, Newtonsoft.Json.JsonConvert.SerializeObject(roles.Select(x => x.Value)));

            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            
            context.Validated(ticket);
           
        }

        public static AuthenticationProperties CreateProperties(string userName, string Roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
        {
            { "userName", userName },
            {"roles",Roles}
        };
            return new AuthenticationProperties(data);
        }
    }
}