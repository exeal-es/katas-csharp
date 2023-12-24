using System;
using System.Collections.Generic;
using Exeal.Katas.TellDontAsk.Domain;
using Exeal.Katas.TellDontAsk.Exception;
using Exeal.Katas.TellDontAsk.Repository;
using Exeal.Katas.TellDontAsk.Tests.Doubles;
using Exeal.Katas.TellDontAsk.UseCase;
using FluentAssertions;
using Xunit;

namespace Exeal.Katas.TellDontAsk.Tests.UseCase
{
    public class OrderCreationUseCaseTest
    {
        private readonly TestOrderRepository orderRepository;
        private readonly ProductCatalog productCatalog;
        private readonly OrderCreationUseCase useCase;
        private readonly Category food;

        public OrderCreationUseCaseTest()
        {
            food = new Category
            {
                Name = "food",
                TaxPercentage = 10M
            };
            orderRepository = new TestOrderRepository();
            productCatalog = new InMemoryProductCatalog(
                new List<Product>
                {
                    new Product
                    {
                        Name = "salad",
                        Price = 3.56M,
                        Category = food,
                    },
                    new Product
                    {
                        Name = "tomato",
                        Price = 4.65M,
                        Category = food
                    }
                }
            );
            useCase = new OrderCreationUseCase(orderRepository, productCatalog);
        }

        [Fact]
        public void SellMultipleItems()
        {
            SellItemRequest saladRequest = new SellItemRequest();
            saladRequest.ProductName = "salad";
            saladRequest.Quantity = 2;

            SellItemRequest tomatoRequest = new SellItemRequest();
            tomatoRequest.ProductName = "tomato";
            tomatoRequest.Quantity = 3;

            SellItemsRequest request = new SellItemsRequest();
            request.Requests = new List<SellItemRequest>();
            request.Requests.Add(saladRequest);
            request.Requests.Add(tomatoRequest);

            useCase.Run(request);

            Order insertedOrder = orderRepository.GetSavedOrder();
            insertedOrder.Status.Should().Be(OrderStatus.Created);
            insertedOrder.Total.Should().Be(23.20M);
            insertedOrder.Tax.Should().Be(2.13M);
            insertedOrder.Currency.Should().Be("EUR");
            insertedOrder.Items.Should().HaveCount(2);
            insertedOrder.Items[0].Product.Name.Should().Be("salad");
            insertedOrder.Items[0].Product.Price.Should().Be(3.56M);
            insertedOrder.Items[0].Quantity.Should().Be(2);
            insertedOrder.Items[0].TaxedAmount.Should().Be(7.84M);
            insertedOrder.Items[0].Tax.Should().Be(0.72M);
            insertedOrder.Items[1].Product.Name.Should().Be("tomato");
            insertedOrder.Items[1].Product.Price.Should().Be(4.65M);
            insertedOrder.Items[1].Quantity.Should().Be(3);
            insertedOrder.Items[1].TaxedAmount.Should().Be(15.36M);
            insertedOrder.Items[1].Tax.Should().Be(1.41M);
        }

        [Fact]
        public void UnknownProduct()
        {
            SellItemsRequest request = new SellItemsRequest();
            request.Requests = new List<SellItemRequest>();
            SellItemRequest unknownProductRequest = new SellItemRequest();
            unknownProductRequest.ProductName = "unknown product";
            request.Requests.Add(unknownProductRequest);

            Action action = () => useCase.Run(request);

            action.Should().Throw<UnknownProductException>();
        }
    }
}