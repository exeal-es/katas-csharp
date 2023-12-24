using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Exception;
using Exeal.Katas.TellDontAsk.Repository;

namespace Exeal.Katas.TellDontAsk.UseCase
{
    public class OrderApprovalUseCase
    {
        private readonly OrderRepository orderRepository;

        public OrderApprovalUseCase(OrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public void Run(OrderApprovalRequest request)
        {
            Order order = orderRepository.GetById(request.OrderId);

            if (order.Status.Equals(OrderStatus.Shipped))
            {
                throw new ShippedOrdersCannotBeChangedException();
            }

            if (request.Approved && order.Status.Equals(OrderStatus.Rejected))
            {
                throw new RejectedOrderCannotBeApprovedException();
            }

            if (!request.Approved && order.Status.Equals(OrderStatus.Approved))
            {
                throw new ApprovedOrderCannotBeRejectedException();
            }

            order.Status = request.Approved ? OrderStatus.Approved : OrderStatus.Rejected;
            orderRepository.Save(order);
        }
    }
}