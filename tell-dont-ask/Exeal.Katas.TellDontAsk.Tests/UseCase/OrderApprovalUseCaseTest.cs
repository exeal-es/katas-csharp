using System;
using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Exception;
using Exeal.Katas.TellDontAsk.Tests.Doubles;
using Exeal.Katas.TellDontAsk.UseCase;
using FluentAssertions;
using Xunit;

namespace Exeal.Katas.TellDontAsk.Tests.UseCase
{
    public class OrderApprovalUseCaseTest
    {
        private readonly TestOrderRepository orderRepository;
        private readonly OrderApprovalUseCase useCase;

        public OrderApprovalUseCaseTest()
        {
            orderRepository = new TestOrderRepository();
            useCase = new OrderApprovalUseCase(orderRepository);
        }

        [Fact]
        public void ApprovedExistingOrder()
        {
            Order initialOrder = new Order();
            initialOrder.Status = OrderStatus.Created;
            initialOrder.Id = 1;
            orderRepository.AddOrder(initialOrder);

            OrderApprovalRequest request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = true;

            useCase.Run(request);

            Order savedOrder = orderRepository.GetSavedOrder();
            savedOrder.Status.Should().Be(OrderStatus.Approved);
        }

        [Fact]
        public void RejectedExistingOrder()
        {
            Order initialOrder = new Order();
            initialOrder.Status = OrderStatus.Created;
            initialOrder.Id = 1;
            orderRepository.AddOrder(initialOrder);

            OrderApprovalRequest request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = false;

            useCase.Run(request);

            Order savedOrder = orderRepository.GetSavedOrder();
            savedOrder.Status.Should().Be(OrderStatus.Rejected);
        }

        [Fact]
        public void CannotApproveRejectedOrder()
        {
            Order initialOrder = new Order();
            initialOrder.Status = OrderStatus.Rejected;
            initialOrder.Id = 1;
            orderRepository.AddOrder(initialOrder);

            OrderApprovalRequest request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = true;

            Action action = () => useCase.Run(request);

            action.Should().Throw<RejectedOrderCannotBeApprovedException>();
            orderRepository.GetSavedOrder().Should().BeNull();
        }

        [Fact]
        public void CannotRejectApprovedOrder()
        {
            Order initialOrder = new Order();
            initialOrder.Status = OrderStatus.Approved;
            initialOrder.Id = 1;
            orderRepository.AddOrder(initialOrder);

            OrderApprovalRequest request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = false;

            Action action = () => useCase.Run(request);

            action.Should().Throw<ApprovedOrderCannotBeRejectedException>();
            orderRepository.GetSavedOrder().Should().BeNull();
        }

        [Fact]
        public void ShippedOrdersCannotBeApproved()
        {
            Order initialOrder = new Order();
            initialOrder.Status = OrderStatus.Shipped;
            initialOrder.Id = 1;
            orderRepository.AddOrder(initialOrder);

            OrderApprovalRequest request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = true;

            Action action = () => useCase.Run(request);

            action.Should().Throw<ShippedOrdersCannotBeChangedException>();
            orderRepository.GetSavedOrder().Should().BeNull();
        }

        [Fact]
        public void ShippedOrdersCannotBeRejected()
        {
            Order initialOrder = new Order();
            initialOrder.Status = OrderStatus.Shipped;
            initialOrder.Id = 1;
            orderRepository.AddOrder(initialOrder);

            OrderApprovalRequest request = new OrderApprovalRequest();
            request.OrderId = 1;
            request.Approved = false;

            Action action = () => useCase.Run(request);

            action.Should().Throw<ShippedOrdersCannotBeChangedException>();
            orderRepository.GetSavedOrder().Should().BeNull();
        }
    }
}