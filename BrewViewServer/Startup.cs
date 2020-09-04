using BrewViewServer.GraphQL;
using BrewViewServer.Models;
using BrewViewServer.Repositories;
using BrewViewServer.Services;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                //opt.UseSqlite("Data Source=Brew.db");
                opt.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Brew;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });

            //services.AddAuthentication(opt =>
            //{
            //    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer();

            //var builder = services.AddIdentityCore<AppUser>(o =>
            //    {
            //        o.Password.RequireDigit = false;
            //        o.Password.RequireLowercase = false;
            //        o.Password.RequireUppercase = false;
            //        o.Password.RequireNonAlphanumeric = false;
            //        o.Password.RequiredLength = 6;
            //    })
            //    .AddEntityFrameworkStores<BrewContext>();


            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .AddAuthorizeDirectiveType()
                    .Create());

            services.AddScoped<TokenService>();
            services.AddScoped<IBrewRepository, BrewRepository>();
            services.AddSingleton<IGoogleRepository, GoogleRepository>();
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

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseGraphQL();
            app.UsePlayground();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}