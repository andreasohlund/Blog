namespace MyAuctionSite.Billing
{
    using System;
    using NServiceBus;

    public interface MemberBilled:IMessage
    {
        Guid MemberId { get; set; }
        double Total { get; set; }
    }
}
