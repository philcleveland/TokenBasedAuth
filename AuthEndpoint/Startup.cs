using AuthEndpoint.Models;
using AuthEndpoint.Providers;
using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartup(typeof(AuthEndpoint.Startup))]
namespace AuthEndpoint
{
    public class Startup
    {
        public AutofacWebApiDependencyResolver Kernal { get; set; }
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();

            var connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterType<AuthRepository>().As<IAuthRepository>();

            builder.RegisterType<SQLBasedUserStore<User>>()
                .AsImplementedInterfaces()
                .WithParameter("connectionString", connStr);
                //.InstancePerRequest();

            builder.RegisterType<EmailService>().As<IIdentityMessageService>();

            builder.Register<IdentityFactoryOptions<AppUserManager>>(c => new IdentityFactoryOptions<AppUserManager>() 
                                                                                { 
                                                                                    DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider() 
                                                                                });

            builder.RegisterType<AppUserManager>().AsSelf();
            //.InstancePerRequest();
            builder.RegisterType<AuthorizationServerProvider>().As<IOAuthAuthorizationServerProvider>();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            
            config.DependencyResolver = resolver;

            ConfigureOAuth(app, resolver.GetService(typeof(IOAuthAuthorizationServerProvider)) as IOAuthAuthorizationServerProvider);

            WebApiConfig.Register(config);
            
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            
        }

        private void ConfigureOAuth(IAppBuilder app, IOAuthAuthorizationServerProvider authprovider)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
#if DEBUG
                AllowInsecureHttp = true,
#else
                AllowInsecureHttp = false,
#endif

                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = authprovider
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

    }
}
