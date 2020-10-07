using System.Reflection;
using BrewView.DatabaseModels;
using BrewView.Server.Authentication;
using BrewView.Server.Authentication.BrewView;
using BrewView.Server.Authentication.Google;
using BrewView.Server.GraphQL;
using BrewView.Server.Repositories;
using BrewView.Server.Repositories.Abstractions;
using BrewView.Server.Services;
using BrewView.Server.Services.Abstractions;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AuthenticationService = BrewView.Server.Authentication.AuthenticationService;

namespace BrewView.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BrewContext>(opt =>
            {
#if DEBUG
                opt.UseSqlite("Data Source=../Brew.db",
                    builder => builder.MigrationsAssembly(Assembly.GetAssembly(typeof(Startup)).ToString()));
#else
                opt.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"), builder => builder.MigrationsAssembly(Assembly.GetAssembly(typeof(Startup)).ToString()));
#endif
            });

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .Create());

            services.AddScoped<IBrewViewAuthentication, BrewViewAuthentication>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOAuthService, OAuthService>();
            services.AddScoped<IBrewRepository, BrewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IGoogleAuthentication, GoogleAuthentication>();
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddHttpClient();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.AddAzureWebAppDiagnostics();
                builder.AddApplicationInsights();
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseExceptionHandler("/error/dev");
            else app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCustomAuthentication();

            app.UseGraphQL("/graphql");

            //if (env.IsDevelopment()) app.UsePlayground(new PlaygroundOptions(){QueryPath = "/g"});

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}