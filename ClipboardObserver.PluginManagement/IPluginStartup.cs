using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardObserver.PluginManagement
{
    public interface IPluginStartup
    {
        Action<IServiceCollection, IConfiguration> Register(IServiceCollection services, IConfigurationBuilder cfgBuilder);
    }

    public interface IPluginMenuItem
    {
        string HandlerName { get; }

        ToolStripMenuItem GetMenuItem();
    }
}
