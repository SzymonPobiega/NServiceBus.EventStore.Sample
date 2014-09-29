namespace VideoStore.Common
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.MessageMutator;

    public class UserIdMutator : IMutateTransportMessages, INeedInitialization
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            userId.Value = transportMessage.Headers.ContainsKey("UserId") ? transportMessage.Headers["UserId"] : "";           
        }

        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            transportMessage.Headers["UserId"] = userId.Value ?? "";
        }

        public void Init()
        {
            Configure.Instance.Configurer.ConfigureComponent<UserIdMutator>(DependencyLifecycle.InstancePerCall);
        }

        static readonly ThreadLocal<string> userId = new ThreadLocal<string>();
    }
}
