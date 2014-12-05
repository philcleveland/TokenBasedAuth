using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;

namespace AuthEndpoint
{
    public static class AuthenticationDatabase
    {
        public static string LocalizeSQLiteConnection(ConnectionStringSettings csSettings)
        {
            var builder = new SqlConnectionStringBuilder(csSettings.ConnectionString);
            string url = System.Web.HttpContext.Current.Server.MapPath(builder.DataSource);
            builder.DataSource = url;
            return builder.ToString();
        }

        public static void InitializeSQLiteDatabase(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            string url = builder.DataSource;
            //var conn = string.Format("Data Source={0};Version=3;", url);

            if (!Directory.Exists(Path.GetDirectoryName(url)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(url));
            }

            if (!File.Exists(url))
            {
                SQLiteConnection.CreateFile(url);
            }

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
