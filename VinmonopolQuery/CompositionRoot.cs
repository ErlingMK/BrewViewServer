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
                var builder = Program.AppSettings.UseSqlite ? new DbContextOptionsBuilder().UseSqlite(Program.AppSettings.DbConnectionSqlite) : new DbContextOptionsBuilder().UseSqlServer(Program.AppSettings.DbConnection);
                var brewContext = new BrewContext(builder.Options);
                return brewContext;
            });
            serviceRegistry.Register<IVinmonopolService, VinmonopolService>();
        }
    }
}