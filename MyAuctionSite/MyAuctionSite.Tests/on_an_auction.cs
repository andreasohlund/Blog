namespace MyAuctionSite.Tests
{
    using System;
    using Backend.Auctions;
    using Events;
    using Machine.Specifications;

    public class on_an_auction : specification_for<Auction>
    {

        Establish context = () =>
            {
                auction = (Auction)aggregateRoot;

                Raise<AuctionRegistered>(a =>
                    {
                        a.AuctionId = Guid.NewGuid();
                    });   
            };

        protected static Auction auction;
    }
}