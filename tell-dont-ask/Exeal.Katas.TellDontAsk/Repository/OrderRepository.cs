using Exeal.Katas.TellDontAsk.Domain;

namespace Exeal.Katas.TellDontAsk.Repository
{
    public interface OrderRepository
    {
        void Save(Order order);

        Order GetById(int orderId);
    }
}
