using AuthEndpoint.Models;
using AuthEndpoint.Providers;
using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Web.Configuration;
using System.Web.Http;

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

            var connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Identity"];
            var connectionString = AuthenticationDatabase.LocalizeSQLiteConnection(connStr);
            AuthenticationDatabase.InitializeSQLiteDatabase(connectionString);

            Kernal = CreateKernel(connectionString);

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
#if DEBUG
                AllowInsecureHttp = true,
#else
                AllowInsecureHttp = false,
#endif

                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider(Kernal.Get<IAuthRepository>())
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public static IKernel CreateKernel(string authDatabaseConnectionString)
        {
            var kernel = new StandardKernel();

            kernel.Bind<IUserStore<User>>()
                .To<SqliteUserStore<User>>()
                .WithConstructorArgument("connectionString", authDatabaseConnectionString);

            var userManager = new UserManager<User>(kernel.Get<IUserStore<User>>());
            userManager.UserLockoutEnabledByDefault = true;
            userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(2);
            userManager.MaxFailedAccessAttemptsBeforeLockout = 5;
            userManager.EmailService = new EmailService();
            //userManager.UserTokenProvider = new DataProtectorTokenProvider<User>(protector);

            kernel.Bind<UserManager<User>>().ToConstant(userManager);
                //.ToSelf();

            kernel.Bind<IAuthRepository>()
                .To<SqliteAuthRepository>();

            return kernel;
        }
    }
}
