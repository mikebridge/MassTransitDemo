using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDemoLib;

namespace ConsumerApp
{
    public class TestTemporaryConsumer : Consumes<TestTemporaryMessage>.All
    {
        public void Consume(TestTemporaryMessage message)
        {
            Console.WriteLine("TEMPORARY Consumer hears: " + message.Message);
        }
    }
}
