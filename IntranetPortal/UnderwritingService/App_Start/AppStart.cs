﻿using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using NLog;
using Owin;
using System.Activities.Statements;
using System.Web.Http;

[assembly: OwinStartup(typeof(RedQ.UnderwritingService.Startup))]
namespace RedQ.UnderwritingService
{
    public class Startup
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {

            // Configure Signarl for self-host. 
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true,
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                map.RunSignalR(hubConfiguration);
                logger.Info("SignalR running.");
            });

            HttpConfiguration config = new HttpConfiguration();
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }
    }
}
