using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using MutationTestingMeetup.Domain;
using MutationTestingMeetup.Tests.Asserters;
using NUnit.Framework;

//TODO: consider hide lastpicked and domain events with using a view model or so
namespace MutationTestingMeetup.Tests.Application.Controllers
{
    public class ProductsControllerTest
    {
        [Test]
        public async Task GetProductShouldReturnNotFound_whenNoProductExist()
        {
            using var scope = new InMemoryTestServerScope();

            var response = await scope.Client.GetAsync($"/products/{Guid.NewGuid()}");

            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetProductShouldProduct_whenProductExist()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var product = new Product("Logitech HD Pro Webcam", ProductCategory.Electronic, 200, false);
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
                isOnSale = false,
                lastPickedOn = (object)null, //TODO: consider hide these?! check other tests
                domainEvents = Array.Empty<object>()
        });
        }

        [Test]
        public async Task GetAllShouldReturnBadRequest_whenNoCategorySpecified()
        {
            using var scope = new InMemoryTestServerScope();

            var response = await scope.Client.GetAsync("/products");

            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.BadRequest);
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
        public async Task GetAllShouldReturnProduct_whenSingleFoundInCategory()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var product = new Product("Logitech HD Pro Webcam", ProductCategory.Electronic, 300, true);
            await scope.AddProductsToDbContext(product);

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
                    isOnSale = true,
                    lastPickedOn = (object)null,
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
                isOnSasle = false
            };

            //Act
            var response = await scope.Client.PostAsync("/products", JsonPayloadBuilder.Build(newProduct));

            //Assert
            await HttpResponseMessageAsserter.AssertThat(response).HasStatusCode(HttpStatusCode.Created);

            scope.WebShopDbContext.Products.Should().SatisfyRespectively(
                p =>
                {
                    p.Name.Should().Be("DEWALT Screwdriver Bit Set");
                    p.Category.Should().Be(ProductCategory.Tool);
                    p.Price.Should().Be(700);
                    p.IsOnSale.Should().BeFalse();
                });
        }

        [Test]
        public async Task ShouldReturnConflict_whenProductWithNameAlreadyExists()
        {
            //Arrange
            using var scope = new InMemoryTestServerScope();

            var productName = "Logitech HD Pro Webcam";

            var product = new Product(productName, ProductCategory.Electronic, 200, false);
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
    }
}