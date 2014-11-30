using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using AuthEndpoint.Providers;
using Microsoft.Owin.Cors;
using Raven.Client;
using Raven.Client.Embedded;
using Ninject;

[assembly: OwinStartup(typeof(AuthEndpoint.Startup))]
namespace AuthEndpoint
{
    public class Startup
    {
        public IKernel Kernal { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();
            Kernal = CreateKernel();
            config.DependencyResolver = new NinjectDependencyResolver(Kernal);

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = false,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider(Kernal.Get<IAuthRepository>())
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
        
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load<RavenModule>();
            kernel.Bind<IAuthRepository>().To<RavenDBAuthRepository>();

            return kernel;
        }
    }
}
