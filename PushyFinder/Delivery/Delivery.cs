using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushyFinder.Delivery
{
    internal interface IDelivery
    {
        public abstract static String Name { get; }
        public abstract static void Deliver(string title, string text = "");
    }

    public enum deliveries
    {
        Pushover,
        Ntfy,
    }

    public static class DeliveryManager
    {
        public static Action<string, string> Deliver()
        {
            return Plugin.Configuration.DeliveryService switch
            {
                deliveries.Pushover => PushoverDelivery.Deliver,
                deliveries.Ntfy => NtfyDelivery.Deliver,
                _ => throw new NotImplementedException($"Unsupported Delivery Destination {Plugin.Configuration.DeliveryService}"),
            };
        }
    }
}
