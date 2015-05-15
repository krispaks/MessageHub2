using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Microsoft.Practices.Unity.WebApi;

[assembly: OwinStartup(typeof(MessageHub.Web.Startup))]

namespace MessageHub.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			HttpConfiguration config = new HttpConfiguration();

			WebApiConfig.Register(config);

			//ConfigureAuth(app);

			config.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());

			app.UseWebApi(config);
        }
    }
}
