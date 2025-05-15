using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClipboardObserver.Common
{
    public static class GlobalInstances
    {
        public static IConfiguration Configuration { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }
    }
}
