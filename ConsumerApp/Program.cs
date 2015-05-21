using System;
using System.Collections.Generic;

using Topshelf;

namespace ConsumerApp
{
    public class Program
    {
        public class ProgramStartStop
        {
            public IList<IConsumerSession> ConsumerSessions = new List<IConsumerSession>();

   
            public void Start() {
                Console.WriteLine("Consumer App Starting...");
                ConsumerSessions.Add(new ConsumerSession<TestTemporaryConsumer>("TemporaryQueue.consumer", permanent: false));
                ConsumerSessions.Add(new ConsumerSession<TestPermanentConsumer>("PermanentQueue.consumer", permanent: true));

                foreach (var consumerSession in ConsumerSessions)
                {
                    consumerSession.Subscribe();
                }

            }

            public void Stop()
            {
                Console.WriteLine("Consumer App Stopping...");
                foreach (var consumerSession in ConsumerSessions)
                {
                    consumerSession.Dispose();
                }
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

                x.SetDescription("Consumer Console");
                x.SetDisplayName("ConsumerApp");
                x.SetServiceName("ConsumerApp");

                x.DependsOn("RabbitMQ");
            });
        }
    }
}
