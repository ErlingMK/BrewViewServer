using System.Net.Http;
using BrewView.DatabaseModels;
using LightInject;
using Microsoft.EntityFrameworkCore;
using VinmonopolQuery.Services;
using VinmonopolQuery.Util;

namespace VinmonopolQuery
{
    internal class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<HttpClient>(new PerContainerLifetime());
            serviceRegistry.Register(factory =>
            {
#if DEBUG
                var builder = new DbContextOptionsBuilder().UseSqlite(Program.AppSettings.DbConnectionSqlite);
#else
                var builder = new DbContextOptionsBuilder().UseSqlServer(Program.AppSettings.DbConnection);
#endif
                var brewContext = new BrewContext(builder.Options);
                return brewContext;
            });
            serviceRegistry.Register<IVinmonopolService, VinmonopolService>();
        }
    }
}