using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BrewView.DatabaseModels;
using LightInject;
using Microsoft.EntityFrameworkCore;
using VinmonopolQuery.Services;
using VinmonopolQuery.Util;

namespace VinmonopolQuery
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var argDict = new Dictionary<string, string>();
            try
            {
                for (var i = 0; i < args.Length; i += 2)
                {
                    argDict.Add(args[i], args[i + 1]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var serviceContainer = new ServiceContainer(options => options.EnablePropertyInjection = false);
            serviceContainer.RegisterFrom<CompositionRoot>();

            var vinmonopolService = serviceContainer.GetInstance<IVinmonopolService>();

            return await vinmonopolService.GetProducts(bool.Parse(argDict["-mode"]), argDict["-since"]);
        }
    }

    internal class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<HttpClient>(new PerContainerLifetime());
            serviceRegistry.Register(factory =>
            {
#if DEBUG
                var builder = new DbContextOptionsBuilder().UseSqlite(AppConstants.DbConnectionSqlite);
#else
                var builder = new DbContextOptionsBuilder().UseSqlServer(AppConstants.DbConnection);
#endif
                var brewContext = new BrewContext(builder.Options);
                return brewContext;
            });
            serviceRegistry.Register<IVinmonopolService, VinmonopolService>();
        }
    }
}
