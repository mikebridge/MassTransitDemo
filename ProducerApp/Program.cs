using System;
using System.Collections.Generic;
using System.Threading;
using MassTransit;
using MassTransitDemoLib;
using Topshelf;

namespace ProducerApp
{
    public class Program
    {
        public class ProgramStartStop
        {
            readonly IList<IServiceBus> _buses = new List<IServiceBus>();

            public void Start() {

                var permanentBus = BusFactory.CreateBus("PermanentQueue.producer");
                _buses.Add(permanentBus);

                var temporaryBus = BusFactory.CreateBus("TemporaryQueue.producer");
                _buses.Add(temporaryBus);

                for (var i = 1; i <= 3; i++)
                {
                    Console.WriteLine("...Publishing message #" + i);
                    permanentBus.Publish(new TestPermanentMessage { Message = "Permanent Message #" + i });
                    temporaryBus.Publish(new TestTemporaryMessage { Message = "Temporary Message #" + i });
                    Thread.Sleep(3000);
                }
                Console.Write("Finished Sending Messages");
                
            }
          
            public void Stop() {
                foreach (var bus in _buses)
                {
                    Console.WriteLine("Disposing bus");
                    bus.Dispose();
                }
                Console.WriteLine("STOPPED");
            }
        }

        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ProgramStartStop>(s =>
                {
                    s.ConstructUsing(_ => new ProgramStartStop());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());   
                });

                x.SetDescription("Producer Console");
                x.SetDisplayName("ProducerApp");
                x.SetServiceName("ProducerApp");

                x.DependsOn("RabbitMQ");
            });     
        }

        
    }
}
