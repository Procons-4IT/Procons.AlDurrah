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
    using Procons.Durrah.Facade;

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private string _userType = string.Empty;

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            context.AdditionalResponseParameters.Add("UserType", _userType);
            return base.TokenEndpoint(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                ApplicationUser user = null; ;
                try
                {
                    user = await userManager.FindAsync(context.UserName, context.Password);
                }
                catch (Exception ex)
                {
                    Utilities.LogException(ex);
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
                _userType = user.UserType;
                //ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
                ClaimsIdentity oAuthIdentity = userManager.CreateIdentityAsync(user, "JWT").Result;
                oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
                oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));

                List<Claim> roles = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                AuthenticationProperties properties = CreateProperties(user.UserName, Newtonsoft.Json.JsonConvert.SerializeObject(roles.Select(x => x.Value)));
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);

                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                Utilities.LogException(ex);
            }

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