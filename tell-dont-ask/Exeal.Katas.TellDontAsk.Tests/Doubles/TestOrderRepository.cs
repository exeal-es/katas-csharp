using System.Collections.Generic;
using System.Linq;
using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Repository;

namespace Exeal.Katas.TellDontAsk.Tests.Doubles
{
    public class TestOrderRepository : OrderRepository
    {
        private Order insertedOrder;
        private List<Order> orders = new List<Order>();

        public Order GetSavedOrder()
        {
            return insertedOrder;
        }

        public void Save(Order order)
        {
            this.insertedOrder = order;
        }

        public Order GetById(int orderId)
        {
            return orders.First(o => o.Id == orderId);
        }

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }
    }
}
