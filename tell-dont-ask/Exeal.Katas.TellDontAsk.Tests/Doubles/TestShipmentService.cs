using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Service;

namespace Exeal.Katas.TellDontAsk.Tests.Doubles
{
    public class TestShipmentService : ShipmentService
    {
        private Order shippedOrder = null;

        public Order GetShippedOrder()
        {
            return shippedOrder;
        }

        public void Ship(Order order)
        {
            shippedOrder = order;
        }
    }
}
