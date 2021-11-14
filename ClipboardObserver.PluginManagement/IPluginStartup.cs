using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WK.Libraries.SharpClipboardNS;

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
