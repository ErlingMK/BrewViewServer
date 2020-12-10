using System.Reflection;
using AutoMapper;
using BrewView.Contracts;
using BrewView.DatabaseModels;
using BrewView.DatabaseModels.Vinmonopol;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AuthenticationService = BrewView.Server.Authentication.AuthenticationService;
using Basic = BrewView.DatabaseModels.Vinmonopol.Basic;
using Characteristics = BrewView.DatabaseModels.Vinmonopol.Characteristics;
using Classification = BrewView.DatabaseModels.Vinmonopol.Classification;
using Description = BrewView.DatabaseModels.Vinmonopol.Description;
using Food = BrewView.DatabaseModels.Vinmonopol.Food;
using Grape = BrewView.DatabaseModels.Vinmonopol.Grape;
using Ingredients = BrewView.DatabaseModels.Vinmonopol.Ingredients;
using LastChanged = BrewView.DatabaseModels.Vinmonopol.LastChanged;
using Logistics = BrewView.DatabaseModels.Vinmonopol.Logistics;
using Origin = BrewView.DatabaseModels.Vinmonopol.Origin;
using Origins = BrewView.DatabaseModels.Vinmonopol.Origins;
using Price = BrewView.DatabaseModels.Vinmonopol.Price;
using Production = BrewView.DatabaseModels.Vinmonopol.Production;
using Properties = BrewView.DatabaseModels.Vinmonopol.Properties;

namespace BrewView.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BrewContext>(opt =>
            {
#if DEBUG
                //opt.UseSqlite("Data Source=../Brew.db",
                //    builder => builder.MigrationsAssembly(Assembly.GetAssembly(typeof(Startup)).ToString()));
                opt.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"), builder => builder.MigrationsAssembly(Assembly.GetAssembly(typeof(Startup)).ToString()));
#else
                opt.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"),
                    builder => builder.MigrationsAssembly(Assembly.GetAssembly(typeof(Startup)).ToString()));
#endif
            });

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .Create());

            services.AddSingleton(new MapperConfiguration(ConfigureMapper));
            services.AddScoped(provider => provider.GetService<MapperConfiguration>().CreateMapper());

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

        public static void ConfigureMapper(IMapperConfigurationExpression obj)
        {
            obj.CreateMap<AlcoholicEntity, Brew>();
            obj.CreateMap<Basic, Contracts.Basic>();
            obj.CreateMap<Logistics, Contracts.Logistics>();

            obj.CreateMap<Origins, Contracts.Origins>();
            obj.CreateMap<Origin, Contracts.Origin>();
            obj.CreateMap<Production, Contracts.Production>();

            obj.CreateMap<Properties, Contracts.Properties>();
            obj.CreateMap<Classification, Contracts.Classification>();

            obj.CreateMap<Ingredients, Contracts.Ingredients>();
            obj.CreateMap<Grape, Contracts.Grape>();

            obj.CreateMap<Description, Contracts.Description>();
            obj.CreateMap<Characteristics, Contracts.Characteristics>();
            obj.CreateMap<Food, Contracts.Food>();

            obj.CreateMap<Price, Contracts.Price>();
            obj.CreateMap<LastChanged, Contracts.LastChanged>();
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