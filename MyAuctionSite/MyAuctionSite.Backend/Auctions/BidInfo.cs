namespace MyAuctionSite.Backend.Auctions
{
    using System;

    public class BidInfo : IEquatable<BidInfo>
    {
        public Guid BidId { get; set; }

        public double Amount { get; set; }


        public bool Equals(BidInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.BidId.Equals(BidId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(BidInfo)) return false;
            return Equals((BidInfo)obj);
        }

        public override int GetHashCode()
        {
            return BidId.GetHashCode();
        }

        public static bool operator ==(BidInfo left, BidInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BidInfo left, BidInfo right)
        {
            return !Equals(left, right);
        }
    }
}