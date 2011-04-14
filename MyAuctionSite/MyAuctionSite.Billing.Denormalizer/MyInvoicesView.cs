namespace MyAuctionSite.Billing.Denormalizer
{
    using System;
    using Messages;
    using NServiceBus;

    public class MyInvoicesView: 
        IHandleMessages<MemberBilled>,
        IHandleMessages<InvoicePaid>
    {
        public void Handle(MemberBilled message)
        {
            //add row to PWM
        }

        public void Handle(InvoicePaid message)
        {
            //set status to Paid
        }
    }

    public interface InvoicePaid : IMessage
    {
         Guid InvoiceId { get; set; }
    }
}
