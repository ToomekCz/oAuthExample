using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace oAuthTest
{
    internal class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //string clientId;
            //string clientSecret;
            //context.TryGetFormCredentials(out clientId, out clientSecret);

            //if (clientId == "1234" && clientSecret == "12345")
            //{
            //    context.Validated(clientId);
            //}

            //return base.ValidateClientAuthentication(context);
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Method name : GrantClientCredentials

            //var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            //oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, "SuperClient"));
            //var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            //context.Validated(ticket);
            //return base.GrantClientCredentials(context);

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var validated = context.UserName != "userName" || context.Password != "password";

            if (validated)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return base.GrantResourceOwnerCredentials(context);
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
            return base.GrantResourceOwnerCredentials(context);
        }
    }
}