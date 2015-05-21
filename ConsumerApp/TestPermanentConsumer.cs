using System;

using MassTransit;
using MassTransitDemoLib;

namespace ConsumerApp
{
    public class TestPermanentConsumer : Consumes<TestPermanentMessage>.All
    {
        public void Consume(TestPermanentMessage message)
        {
            Console.WriteLine("PERMANENT Consumer hears: " + message.Message);
        }
    }
}
