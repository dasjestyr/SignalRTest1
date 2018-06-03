using System;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using EndpointConfiguration = NServiceBus.EndpointConfiguration;

namespace SignalRTest1.DependencyInjection
{
    public static class NsbInstaller
    {
        public static void AddNsb(this IServiceCollection services)
        {
            var licensePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NSB_License.xml");
            
            var config = new EndpointConfiguration("SignalRTest");
            if (File.Exists(licensePath))
                config.LicensePath(licensePath);

            config.UseTransport<LearningTransport>();
            
            config.RegisterComponents(
                configComp =>
                {
                    configComp.ConfigureComponent(() =>
                        GetHubContext(services), 
                        DependencyLifecycle.InstancePerCall);
                });

            var session = Endpoint.Start(config).Result;
            services.AddSingleton<IMessageSession>(session);
        }

        private static IHubContext<MessageHub, IClientSystemUpdate> GetHubContext(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            // if NSB were capturing, this would be null every time but it's not
            var xxx = provider.GetService<XXX>();
            
            if(xxx == null)
                services.AddTransient<XXX>();

            // BUG: if you observe client connections here, it says 0 instead of 1 every time
            var hub = provider.GetRequiredService<IHubContext<MessageHub, IClientSystemUpdate>>();
            return hub;
        }
    }

    public class XXX
    {
        public string Value { get; set; } = "Foo";
    }
}
