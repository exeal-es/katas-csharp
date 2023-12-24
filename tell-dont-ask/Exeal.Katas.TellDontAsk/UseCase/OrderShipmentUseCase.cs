using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Exception;
using Exeal.Katas.TellDontAsk.Repository;
using Exeal.Katas.TellDontAsk.Service;

namespace Exeal.Katas.TellDontAsk.UseCase
{
    public class OrderShipmentUseCase
    {
        private readonly OrderRepository orderRepository;
        private readonly ShipmentService shipmentService;

        public OrderShipmentUseCase(OrderRepository orderRepository, ShipmentService shipmentService)
        {
            this.orderRepository = orderRepository;
            this.shipmentService = shipmentService;
        }

        public void Run(OrderShipmentRequest request)
        {
            Order order = orderRepository.GetById(request.OrderId);

            if (order.Status.Equals(OrderStatus.Created) || order.Status.Equals(OrderStatus.Rejected))
            {
                throw new OrderCannotBeShippedException();
            }

            if (order.Status.Equals(OrderStatus.Shipped))
            {
                throw new OrderCannotBeShippedTwiceException();
            }

            shipmentService.Ship(order);

            order.Status = OrderStatus.Shipped;
            orderRepository.Save(order);
        }
    }
}
