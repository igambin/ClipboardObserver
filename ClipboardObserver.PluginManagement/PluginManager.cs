using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WK.Libraries.SharpClipboardNS;

namespace ClipboardObserver.PluginManagement
{
    public class PluginManager 
    {
        private IServiceProvider ServiceProvider { get; set; }
        
        public PluginManager() { }

        public List<Action<IServiceCollection, IConfiguration>> Startup(IServiceCollection services, IConfigurationBuilder configBuilder)
        {
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

            var dlls = 
                Directory
                    .GetFiles(path, "*.dll", SearchOption.AllDirectories)
                    .Where(dll => new FileInfo(dll).Name.StartsWith($"ClipboardObserver.Plugins"))
                    .ToList();

            List<Assembly> assemblies = new List<Assembly>();
            
            assemblies.AddRange(dlls.Select(Assembly.LoadFile));

            List<Type> plugins = 
                assemblies
                    .SelectMany(x => x.GetTypes())
                    .Where(y => typeof(IPluginStartup).IsAssignableFrom(y) && !y.IsInterface)
                    .ToList();

            var optionsConfigurators = new List<Action<IServiceCollection, IConfiguration>>();

            plugins.ForEach(tPlugin =>
            {
                IPluginStartup plugin = (IPluginStartup)Activator.CreateInstance(tPlugin);
                var action = plugin?.Register(services, configBuilder);
                if (action != null) optionsConfigurators.Add(action);
            });

            return optionsConfigurators;

        }
    }
}
