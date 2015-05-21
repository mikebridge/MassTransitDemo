using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Subscriptions.Messages;
using MassTransitDemoLib;

namespace ConsumerApp
{
    public interface IConsumerSession: IDisposable
    {
        void Subscribe();
    }

    public class ConsumerSession<T> : IConsumerSession where T : class, IConsumer, new()
    {
        private readonly string _queueName;
        private readonly bool _permanent;
        private IServiceBus _serviceBus;
        private UnsubscribeAction _unsubscribeAction;

        public ConsumerSession(String queueName, bool permanent)
        {
            _queueName = queueName;
            _permanent = permanent;
          
        }

        public void Subscribe()
        {
            // VERSION 1
            if (_permanent)
            {
                Console.WriteLine("Creating PERMANENT bus");
                _serviceBus = BusFactory.CreateBus(_queueName, x => x.Subscribe(s => s.Consumer<T>().Permanent()));
            }
            else
            {
                Console.WriteLine("Creating TEMPORARY bus");
                _serviceBus = BusFactory.CreateBus(_queueName, x => x.Subscribe(s => s.Consumer<T>().Transient()));
            }            

            // VERSION 2 
//            Console.WriteLine("Subscribing " + _queueName);
//            _serviceBus = BusFactory.CreateBus(_queueName);
//            _unsubscribeAction = _serviceBus.SubscribeConsumer<T>();
        }

        public void Dispose()
        {
            // if using Version 2
            if (_unsubscribeAction != null && !_permanent)
            {
                Console.WriteLine("Calling Unsubscribe Action for  " + _queueName);
                _unsubscribeAction();
            }
            Console.WriteLine("Disposing Bus for " + _queueName);
            _serviceBus.Dispose();
        }
    }
}
