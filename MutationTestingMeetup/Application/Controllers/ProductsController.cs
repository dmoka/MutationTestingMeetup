using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Application.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductsFinder _productsFinder;

        public ProductsController(IUnitOfWork unitOfWork, IProductsFinder productsFinder)
        {
            _unitOfWork = unitOfWork;
            _productsFinder = productsFinder;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _unitOfWork.Products.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductsQueryParameters queryParameters)
        {
            if (!queryParameters.Category.HasValue)
            {
                return BadRequest("The product category must be specified");
            }

            var products = await _productsFinder.Find(queryParameters);

            return Ok(products);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductPayload productPayload)
        {

            if (_unitOfWork.Products.Exists(productPayload.Name))
            {
                return StatusCode(409);
            }

            var saleState = productPayload.IsOnSale ? SaleState.OnSale : SaleState.NoSale;
            var product = new Product(productPayload.Name, productPayload.Category, productPayload.Price,
                saleState);

            var storedProduct = _unitOfWork.Products.Create(product);

            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetProduct", new { id = storedProduct.Id }, storedProduct);
        }


        [HttpPost]
        [Route("{id}/pick")]
        public async Task<IActionResult> PickProduct(Guid id, PickPayload payload)
        {
            try
            {
                var product = await _unitOfWork.Products.GetAsync(id);
                product.Pick(payload.Count);
                await _unitOfWork.CommitAsync();
            }
            catch (ApplicationException e)
            {
                return BadRequest(e.Message);
            }


            return StatusCode(204);
        }
    }
}
