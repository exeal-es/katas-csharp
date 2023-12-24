using System;
using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Exception;
using Exeal.Katas.TellDontAsk.Tests.Doubles;
using Exeal.Katas.TellDontAsk.UseCase;
using FluentAssertions;
using Xunit;

namespace Exeal.Katas.TellDontAsk.Tests.UseCase
{
    public class OrderShipmentUseCaseTest
    {
        private readonly TestOrderRepository orderRepository;
        private readonly TestShipmentService shipmentService;
        private readonly OrderShipmentUseCase useCase;

        public OrderShipmentUseCaseTest()
        {
            orderRepository = new TestOrderRepository();
            shipmentService = new TestShipmentService();
            useCase = new OrderShipmentUseCase(orderRepository, shipmentService);
        }

        [Fact]
        public void shipApprovedOrder()
        {
            Order initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.Approved;
            orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            useCase.Run(request);

            orderRepository.GetSavedOrder().Status.Should().Be(OrderStatus.Shipped);
            shipmentService.GetShippedOrder().Should().BeSameAs(initialOrder);
        }

        [Fact]
        public void createdOrdersCannotBeShipped()
        {
            Order initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.Created;
            orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            Action action = () => useCase.Run(request);
            action.Should().Throw<OrderCannotBeShippedException>();

            orderRepository.GetSavedOrder().Should().BeNull();
            shipmentService.GetShippedOrder().Should().BeNull();
        }

        [Fact]
        public void rejectedOrdersCannotBeShipped()
        {
            Order initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.Rejected;
            orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            Action action = () => useCase.Run(request);
            action.Should().Throw<OrderCannotBeShippedException>();

            orderRepository.GetSavedOrder().Should().BeNull();
            shipmentService.GetShippedOrder().Should().BeNull();
        }

        [Fact]
        public void shippedOrdersCannotBeShippedAgain()
        {
            Order initialOrder = new Order();
            initialOrder.Id = 1;
            initialOrder.Status = OrderStatus.Shipped;
            orderRepository.AddOrder(initialOrder);

            OrderShipmentRequest request = new OrderShipmentRequest();
            request.OrderId = 1;

            Action action = () => useCase.Run(request);
            action.Should().Throw<OrderCannotBeShippedTwiceException>();

            orderRepository.GetSavedOrder().Should().BeNull();
            shipmentService.GetShippedOrder().Should().BeNull();
        }
    }
}