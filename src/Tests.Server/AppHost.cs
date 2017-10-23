using System.Collections.Generic;
using Funq;
using ServiceStack;
using ServiceStack.Api.OpenApi;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Validation;
using Tests.ServiceInterface;

namespace Tests
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Client Tests Server", typeof(HelloService).Assembly)
        {
            AppSettings = new MultiAppSettings(
                new EnvironmentVariableSettings(),
                new AppSettings());
        }

        public override void Configure(Container container)
        {
            SetConfig(new HostConfig {
                DebugMode = true,
            });

            Plugins.Add(new OpenApiFeature());
            Plugins.Add(new PostmanFeature());

            container.Register<IDbConnectionFactory>(c =>
                new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            container.Register<IAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()) {
                    UseDistinctRoleTables = AppSettings.Get("UseDistinctRoleTables", true),
                });

            var authRepo = (OrmLiteAuthRepository) container.Resolve<IAuthRepository>();
            authRepo.DropAndReCreateTables();

            CreateUser(authRepo, 1, "test", "test", new List<string> {"TheRole"}, new List<string> {"ThePermission"});
            CreateUser(authRepo, 2, "test2", "test2");

            Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                new IAuthProvider[] {
                    new JwtAuthProvider(AppSettings),
                    new BasicAuthProvider(AppSettings),
                    new CredentialsAuthProvider(AppSettings),
                }));

            Plugins.Add(new ValidationFeature());
            Plugins.Add(new AutoQueryFeature {
                MaxLimit = 100,
            });

            Plugins.Add(new CorsFeature(
                allowOriginWhitelist: new[] {
                    "http://localhost", "http://localhost:5000", "http://tests.servicestack.net",
                    "http://null.jsbin.com"
                },
                allowCredentials: true,
                allowedHeaders: "Content-Type, Allow, Authorization, X-Args"));
        }

        private void CreateUser(OrmLiteAuthRepository authRepo,
            int id, string username, string password, List<string> roles = null, List<string> permissions = null)
        {
            new SaltedHash().GetHashAndSaltString(password, out var hash, out var salt);
            authRepo.CreateUserAuth(new UserAuth {
                Id = id,
                DisplayName = username + " DisplayName",
                Email = username + "@gmail.com",
                UserName = username,
                FirstName = "First " + username,
                LastName = "Last " + username,
                PasswordHash = hash,
                Salt = salt,
                Roles = roles,
                Permissions = permissions
            }, password);

            authRepo.AssignRoles(id.ToString(), roles, permissions);
        }
    }
}