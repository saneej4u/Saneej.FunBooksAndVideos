using Microsoft.AspNetCore.Mvc;
using Saneej.FunBooksAndVideos.Order.Api.Utils;
using Saneej.FunBooksAndVideos.Service.Models;
using Saneej.FunBooksAndVideos.Service.PurchaseOrder;

namespace Saneej.FunBooksAndVideos.Api.Controllers
{
    [ApiController]
    [Route("api/funbooksandvideos/orders/v1.0")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ILogger<PurchaseOrderController> _logger;

        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(ILogger<PurchaseOrderController> logger, IPurchaseOrderService purchaseOrderService)
        {
            _logger = logger;
            _purchaseOrderService = purchaseOrderService;
        }


        [HttpPost]
        [Route("process")]
        public async Task<ActionResult<ResponseWrapper<bool>>> ProcessPurchaseOrderAsync([FromBody] BasketRequest basket)
        {
            var result = await _purchaseOrderService.ProcessOrder(basket);
            return result.ToHttpResponse();
        }
    }
}