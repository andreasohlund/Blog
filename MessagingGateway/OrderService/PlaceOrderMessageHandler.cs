namespace OrderService
{
    using System;
    using System.Net;
    using System.Text;
    using NServiceBus;

    public class PlaceOrderMessageHandler : IHandleMessages<PlaceOrder>
    {
        public void Handle(PlaceOrder message)
        {
            var order = new Order
                            {
                                // use you imagination
                            };
            dataStore.Save(order);

            var webRequest = WebRequest.Create("http://www.fedex.com/registershipment");
            webRequest.Method = WebRequestMethods.Http.Post;

            var payload = "Whatever";

            using (var s = webRequest.GetRequestStream())
                s.Write(Encoding.Default.GetBytes(payload), 0, Encoding.Default.GetByteCount(payload));

            using(var response = webRequest.GetResponse() as HttpWebResponse)
            {
                if(response.StatusCode != HttpStatusCode.OK)
                    throw new Exception();
            }

            //separate to separate endpoint (ShippingService
            // start with IHandleMessage this get's you at least once messaging
            // demand change, more controll of the retry
            // IHandleMessages -> IAmStartedByMessage
        }

        readonly IRepository dataStore;

        public PlaceOrderMessageHandler(IRepository dataStore)
        {
            this.dataStore = dataStore;
        }
    }
}