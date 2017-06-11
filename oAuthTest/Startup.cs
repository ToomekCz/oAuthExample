using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(oAuthTest.Startup))]

namespace oAuthTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );

            ConfigureOAuth(app);

            app.UseWebApi(config);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
           OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
           {
               AllowInsecureHttp = true,
               TokenEndpointPath = new PathString("/token"),
               AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
               Provider = new CustomAuthorizationServerProvider()
           };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
