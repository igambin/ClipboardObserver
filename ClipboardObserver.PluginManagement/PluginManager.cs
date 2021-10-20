using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardObserver.PluginManagement
{
    public class PluginManager
    {

        public PluginManager()
        {
            
        }

        public void RegisterPlugins(IServiceCollection services)
        {
            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var dlls = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories)
                .Where(dll => new FileInfo(dll).Name.StartsWith("ClipboardObserver.Plugins")).ToList();

            List<Assembly> assemblies = new List<Assembly>();
            assemblies.AddRange(dlls.Select(Assembly.LoadFile));

            List<Type> currentAssemblyTypes = assemblies.SelectMany(x => x.GetTypes())
                .Where(y => typeof(IClipboardChangedHandler).IsAssignableFrom(y) && !y.IsInterface).ToList();

            currentAssemblyTypes.ForEach(at => services.AddTransient(typeof(IClipboardChangedHandler), at));
        }
    }
}
