using NServiceBus.Persistence.EventStore.SagaPersister;

namespace VideoStore.Sales
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, UsingTransport<EventStore>, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With().Log4Net();
            Configure.With()
                     .DefaultBuilder()
                     .EventStoreSagaPersister();
            //Configure.With()
            //    .DefaultBuilder()
            //    .RijndaelEncryptionService();
        }
    }


    public class MyClass:IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Console.Out.WriteLine("The VideoStore.Sales endpoint is now started and ready to accept messages");
        }

        public void Stop()
        {
            
        }
    }
}
