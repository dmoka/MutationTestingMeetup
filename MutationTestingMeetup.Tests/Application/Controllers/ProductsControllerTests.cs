using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using MutationTestingMeetup.Application.Controllers;
using MutationTestingMeetup.Domain;
using MutationTestingMeetup.Tests.Asserters;
using NUnit.Framework;

namespace MutationTestingMeetup.Tests.Application.Controllers
{
    public class ProductsControllerTests
    {
        [Test]
        public async Task GetProductShouldReturnNotFound_whenNoProductExist()
        {
            using var scope = new InMemoryTestServerScope();

            var response = await scope.Client.GetAsync($"/products/{Guid.NewGuid()}");

            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetProductShouldReturnProduct_whenProductExist()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var product = new Product("Logitech HD Pro Webcam", ProductCategory.Electronic, 200, SaleState.NoSale);
            await scope.AddProductsToDbContext(product);

            //Act
            var response = await scope.Client.GetAsync($"/products/{product.Id}");

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.OK);
            await HttpResponseMessageAsserter.AssertThat(response).HasJsonInBody(new
            {
                id = product.Id,
                name = "Logitech HD Pro Webcam",
                category = ProductCategory.Electronic,
                price = 200,
                saleState = SaleState.NoSale,
                lastPickState = (int)PickState.New,
                domainEvents = Array.Empty<object>()
            });
        }

        [Test]
        public async Task GetAllProductShouldReturnBadRequest_whenNoCategorySpecified()
        {
            using var scope = new InMemoryTestServerScope();

            var response = await scope.Client.GetAsync("/products");

            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.BadRequest);
            await HttpResponseMessageAsserter.AssertThat(response)
                .HasTextInBody("The product category must be specified");
        }

        [Test]
        public async Task GetAllShouldReturnNoProduct_whenNothingFoundInCategory()
        {
            using var scope = new InMemoryTestServerScope();

            var response = await scope.Client.GetAsync("/products?category=Electronic");

            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.OK);
            await HttpResponseMessageAsserter.AssertThat(response).HasEmptyJsonArrayInBody();
        }

        [Test]
        public async Task GetAllShouldReturnSingleProduct_whenSingleFoundWithFilters()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var product = new Product("Logitech HD Pro Webcam", ProductCategory.Electronic, 300, SaleState.OnSale);
            await scope.AddProductsToDbContext(product);

            var product2 = new Product("Acer Webcam", ProductCategory.Electronic, 600, SaleState.OnSale);
            await scope.AddProductsToDbContext(product2);

            //Act
            var response = await scope.Client.GetAsync("/products?category=Electronic&maxPrice=400&isOnSale=true");

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.OK);
            await HttpResponseMessageAsserter.AssertThat(response).HasJsonArrayInBody(new[]
            {
                new
                {
                    id = product.Id,
                    name = "Logitech HD Pro Webcam",
                    category = ProductCategory.Electronic,
                    price = 300,
                    saleState = SaleState.OnSale,
                    lastPickState = (int)PickState.New,
                    domainEvents = Array.Empty<object>()
                }
            });
        }

        [Test]
        public async Task GetAllShouldReturnMultipleProducts_whenMultipleFoundWithFilters()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var product = new Product("Logitech HD Pro Webcam", ProductCategory.Electronic, 300, SaleState.OnSale);
            await scope.AddProductsToDbContext(product);

            var product2 = new Product("Acer Webcam", ProductCategory.Electronic, 400, SaleState.OnSale);
            await scope.AddProductsToDbContext(product2);

            var product3 = new Product("Acer Webcam v2", ProductCategory.Electronic, 300, SaleState.NoSale);
            await scope.AddProductsToDbContext(product3);

            //Act
            var response = await scope.Client.GetAsync("/products?category=Electronic&maxPrice=400&isOnSale=true");

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.OK);
            await HttpResponseMessageAsserter.AssertThat(response).HasJsonArrayInBody(new[]
            {
                new
                {
                    id = product2.Id,
                    name = "Acer Webcam",
                    category = ProductCategory.Electronic,
                    price = 400,
                    saleState = SaleState.OnSale,
                    lastPickState = (int)PickState.New,
                    domainEvents = Array.Empty<object>()
                },

                new
                {
                    id = product.Id,
                    name = "Logitech HD Pro Webcam",
                    category = ProductCategory.Electronic,
                    price = 300,
                    saleState = SaleState.OnSale,
                    lastPickState = (int)PickState.New,
                    domainEvents = Array.Empty<object>()
                }
            });
        }


        [Test]
        public async Task ProductShouldBeCreated_whenNoProductExistsWithName()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var newProduct = new
            {
                name = "DEWALT Screwdriver Bit Set",
                category = ProductCategory.Tool,
                price = 700,
                saleState = SaleState.NoSale
            };

            //Act
            var response = await scope.Client.PostAsync("/products", JsonPayloadBuilder.Build(newProduct));

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.Created);

            scope.WarehouseDbContext.Products.Should().SatisfyRespectively(
                p =>
                {
                    p.Name.Should().Be("DEWALT Screwdriver Bit Set");
                    p.Category.Should().Be(ProductCategory.Tool);
                    p.Price.Should().Be(700);
                    p.SaleState.Should().Be(SaleState.NoSale);
                });

            var product = scope.WarehouseDbContext.Products.Single();
            var stockLevel = scope.WarehouseDbContext.StockLevels.Single();
            stockLevel.Should().NotBeNull();
            stockLevel.ProductId.Should().Be(product.Id);
            stockLevel.Count.Should().Be(10);
        }

        [Test]
        public async Task ShouldReturnConflict_whenProductWithNameAlreadyExists()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var productName = "Logitech HD Pro Webcam";

            var product = new Product(productName, ProductCategory.Electronic, 200, SaleState.NoSale);
            await scope.AddProductsToDbContext(product);

            var newProduct = new
            {
                name = productName,
                category = ProductCategory.Tool,
                price = 700,
                isOnSasle = false
            };

            //Act
            var response = await scope.Client.PostAsync("/products", JsonPayloadBuilder.Build(newProduct));

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.Conflict);
        }

        [TestCase(2,8)]
        [TestCase(10,0)]
        public async Task PickProductShouldDecreaseStockLevel(int pickCount, int expectedStockLevel)
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var product = new Product("Logitech HD Pro Webcam", ProductCategory.Electronic, 200, SaleState.NoSale);
            await scope.AddProductsToDbContext(product);

            //Act
            var response = await scope.Client.PostAsync($"/products/{product.Id}/pick", JsonPayloadBuilder.Build(new PickPayload {Count = pickCount }));

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.NoContent);
            var stockLevel = scope.WarehouseDbContext.StockLevels.Single();
            stockLevel.Count.Should().Be(expectedStockLevel);
        }

        [Test]
        public async Task PickProductShouldReturnError_whenPickedCountIsBiggerThanStockLevel()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var product = new Product("Logitech HD Pro Webcam", ProductCategory.Electronic, 200, SaleState.NoSale);
            await scope.AddProductsToDbContext(product);

            //Act
            var response = await scope.Client.PostAsync($"/products/{product.Id}/pick", JsonPayloadBuilder.Build(new PickPayload { Count = 11 }));

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.BadRequest);
            await HttpResponseMessageAsserter.AssertThat(response)
                .HasTextInBody("Cannot be picked more than stock level");
        }

    }
}