using AuthEndpoint.Providers;
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
using Dapper;

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

            var csConfig = WebConfigurationManager.OpenWebConfiguration("/AuthEndpoint");
            var connStr = csConfig.ConnectionStrings.ConnectionStrings["Identity"];
            var builder = new SqlConnectionStringBuilder(connStr.ConnectionString);

            string url = System.Web.HttpContext.Current.Server.MapPath(builder.DataSource);
            var conn = string.Format("Data Source={0};Version=3;", url);
            if (!File.Exists(url))
            {
                SQLiteConnection.CreateFile(url);
                
            }

            BuildSQLiteDB(conn);
            kernel.Bind<IAuthRepository>().To<SqliteAuthRepository>().WithConstructorArgument("connectionString", conn);

            return kernel;
        }

        private static void BuildSQLiteDB(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    connection.Execute(@"
                                        CREATE TABLE IF NOT EXISTS `roles` (
                                          `Id` TEXT NOT NULL,
                                          `Name` TEXT NOT NULL,
                                          PRIMARY KEY (`Id`));

                                        CREATE TABLE IF NOT EXISTS `users` (
                                          `Id` TEXT NOT NULL,
                                          `Email` TEXT DEFAULT NULL,
                                          `EmailConfirmed` INTEGER NOT NULL,
                                          `PasswordHash` TEXT,
                                          `SecurityStamp` TEXT,
                                          `PhoneNumber` TEXT,
                                          `PhoneNumberConfirmed` INTEGER NOT NULL,
                                          `TwoFactorEnabled` INTEGER NOT NULL,
                                          `LockoutEndDateUtc` datetime DEFAULT NULL,
                                          `LockoutEnabled` INTEGER NOT NULL,
                                          `AccessFailedCount` INTEGER NOT NULL,
                                          `UserName` TEXT NOT NULL,
                                          PRIMARY KEY (`Id`));

                                        CREATE TABLE IF NOT EXISTS `userclaims` (
                                          `Id` INTEGER NOT NULL,
                                          `UserId` TEXT NOT NULL,
                                          `ClaimType` TEXT,
                                          `ClaimValue` TEXT,
                                          PRIMARY KEY (`Id`),
                                          UNIQUE (`Id`),
                                          CONSTRAINT `ApplicationUser_Claims` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
                                        );

                                        CREATE TABLE IF NOT EXISTS `userlogins` (
                                          `LoginProvider` varchar(128) NOT NULL,
                                          `ProviderKey` varchar(128) NOT NULL,
                                          `UserId` varchar(128) NOT NULL,
                                          PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
                                          CONSTRAINT `ApplicationUser_Logins` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
                                        );

                                        CREATE TABLE IF NOT EXISTS  `userroles` (
                                          `UserId` varchar(128) NOT NULL,
                                          `RoleId` varchar(128) NOT NULL,
                                          PRIMARY KEY (`UserId`,`RoleId`),
                                          CONSTRAINT `ApplicationUser_Roles` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
                                          CONSTRAINT `IdentityRole_Users` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
                                        );
                    ");
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    throw;
                }
            }
        }
    }








}
