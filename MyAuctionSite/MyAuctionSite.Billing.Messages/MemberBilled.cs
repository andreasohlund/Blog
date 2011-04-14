using System;
using System.Collections.Generic;

namespace MyAuctionSite.Billing.Messages
{
    public interface MemberBilled:Billing.MemberBilled
    {
        List<Guid> AuctionsBilled { get; set; }
        string DeliveryType { get; set; }
        string LinkToPDF { get; set; }
        Guid InvoiceID { get; set; }
    }
}
