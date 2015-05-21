using System;
using System.ComponentModel;
using MassTransit;
using MassTransit.BusConfigurators;

namespace MassTransitDemoLib
{
    public static class BusFactory
    {
        private static string _baseUrl = "rabbitmq://localhost/";
        private static string _username = "mylogin";
        private static string _password = "mypassword";

        public static IServiceBus CreateBus(String queueName, params Action<ServiceBusConfigurator>[] configurations)
        {
            var uri = _baseUrl + queueName;
            Console.WriteLine("Configuring bus for " + uri);
            return ServiceBusFactory.New(sbc =>
            {
                
                sbc.UseRabbitMq(
                    r => {                        
                        r.ConfigureHost(new Uri(uri),
                                 h => {
                                     h.SetUsername(_username);
                                     h.SetPassword(_password);
                                 });
                    });

                sbc.ReceiveFrom(uri);

                foreach (var action in configurations)
                {
                    action(sbc);
                }

            });
        }
    }
}
