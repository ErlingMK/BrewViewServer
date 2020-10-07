using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LightInject;
using Newtonsoft.Json;
using VinmonopolQuery.Services;

namespace VinmonopolQuery
{
    public class Program
    {
        public static AppSettings AppSettings { get; set; }

        public static async Task<int> Main(string[] args)
        {
            var argDict = new Dictionary<string, string>();
            try
            {
                for (var i = 0; i < args.Length; i += 2) argDict.Add(args[i], args[i + 1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            AppSettings = JsonConvert.DeserializeObject<AppSettings>(await File.ReadAllTextAsync("appsettings.development.json"));

            var serviceContainer = new ServiceContainer(options => options.EnablePropertyInjection = false);
            serviceContainer.RegisterFrom<CompositionRoot>();

            var vinmonopolService = serviceContainer.GetInstance<IVinmonopolService>();

            return await vinmonopolService.GetProducts(bool.Parse(argDict["-mode"]), argDict["-since"]);
        }
    }
}