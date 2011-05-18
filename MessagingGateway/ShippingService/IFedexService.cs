namespace ShippingService
{
    public interface IFedexService
    {
        string BookPickup(Order order);
    }
}