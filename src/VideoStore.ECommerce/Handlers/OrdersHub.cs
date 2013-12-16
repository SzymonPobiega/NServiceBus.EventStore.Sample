using System.Transactions;

namespace VideoStore.ECommerce.Handlers
{
    using System.Threading;
    using Microsoft.AspNet.SignalR;
    using VideoStore.Messages.Commands;
    using NServiceBus;
    using System.Diagnostics;

    public class OrdersHub : Hub
    {
        private static int orderNumber;

        public void CancelOrder(int orderNumber)
        {
            using (var tx = new TransactionScope())
            {
                var command = new CancelOrder
                    {
                        ClientId = Context.ConnectionId,
                        OrderNumber = orderNumber
                    };

                MvcApplication.Bus.SetMessageHeader(command, "Debug", ((bool) Clients.Caller.debug).ToString());

                MvcApplication.Bus.Send(command);
                tx.Complete();
            }
        }

        public void PlaceOrder(string[] videoIds)
        {
            if (((bool)Clients.Caller.debug))
            {
                Debugger.Break();
            }
            using (var tx = new TransactionScope())
            {
                var command = new SubmitOrder
                    {
                        ClientId = Context.ConnectionId,
                        OrderNumber = Interlocked.Increment(ref orderNumber),
                        VideoIds = videoIds,
                        EncryptedCreditCardNumber = "4000 0000 0000 0008",
                        // This property will be encrypted. Therefore when viewing the message in the queue, the actual values will not be shown. 
                        EncryptedExpirationDate = "10/13" // This property will be encrypted.
                    };

                MvcApplication.Bus.SetMessageHeader(command, "Debug", ((bool) Clients.Caller.debug).ToString());

                MvcApplication.Bus.Send(command);
                tx.Complete();
            }
        }
    }
}