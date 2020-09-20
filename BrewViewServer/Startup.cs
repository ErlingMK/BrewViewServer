using BrewViewServer.Authentication;
using BrewViewServer.Authentication.Google;
using BrewViewServer.GraphQL;
using BrewViewServer.Repositories;
using BrewViewServer.Services;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthenticationService = BrewViewServer.Authentication.AuthenticationService;

namespace BrewViewServer
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
                opt.UseSqlite("Data Source=Brew.db");
                //opt.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Brew;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .Create());

            services.AddScoped<TokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBrewRepository, BrewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVinmonopolRepository, VinmonopolRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IGoogleAuthentication, GoogleAuthentication>();
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddHttpClient();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCustomAuthentication();

            app.UseGraphQL();

            if (env.IsDevelopment()) app.UsePlayground();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}