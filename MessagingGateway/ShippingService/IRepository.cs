namespace ShippingService
{
    using System;

    public interface IRepository
    {
        void Save<T>(T order);
        T Get<T>(Guid orderId);
    }
}