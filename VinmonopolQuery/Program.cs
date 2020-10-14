using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LightInject;
using Newtonsoft.Json;
using VinmonopolQuery.Services;
using VinmonopolQuery.Util;

namespace VinmonopolQuery
{
    public class Program
    {
        public static AppSettings AppSettings { get; set; }

        public static async Task<int> Main(string[] args)
        {
            Logger.Log("\n\nStarting new session...");

            var argDict = new Dictionary<string, string>();
            try
            {
                for (var i = 0; i < args.Length; i += 2) argDict.Add(args[i], args[i + 1]);
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}\n{e.StackTrace}");
                throw;
            }

            AppSettings = JsonConvert.DeserializeObject<AppSettings>(await File.ReadAllTextAsync("appsettings.development.json"));

            Logger.Log($"UseSqlite: {AppSettings.UseSqlite}");

            var serviceContainer = new ServiceContainer(options => options.EnablePropertyInjection = false);
            serviceContainer.RegisterFrom<CompositionRoot>();

            var vinmonopolService = serviceContainer.GetInstance<IVinmonopolService>();

            if (argDict.TryGetValue("-since", out var since))
            {
                Logger.Log($"Products changed since: {since}");
            }

            var products = await vinmonopolService.GetProducts(bool.Parse(argDict["-mode"]), since);

            Logger.Log("Done");

            return products;
        }
    }
}