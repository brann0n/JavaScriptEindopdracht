using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(JavaScriptUNO.Startup))]

namespace JavaScriptUNO
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //this starts the signalR stuff
            app.MapSignalR();
        }
    }
}
