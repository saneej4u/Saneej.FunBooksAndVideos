using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Saneej.FunBooksAndVideos.Product.Service.Services.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saneej.FunBooksAndVideos.Product.Api.Controllers
{
    [ApiController]
    [Route("api/funbooksandvideos/products/v1.0")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<bool>> ProcessPurchaseOrderAsync([FromBody] List<int> productsIds)
        {
            return Ok(await _productService.GetAllProductByIdsAsync(productsIds));
        }
    }
}
