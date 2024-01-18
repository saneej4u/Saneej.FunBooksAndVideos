using Saneej.FunBooksAndVideos.Data.Entities;
using Saneej.FunBooksAndVideos.Service.Constants;
using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Extensions
{
    public static class PurchaseOrderLineExtension
    {
        public static PurchaseOrderLine ToPurchaseOrderLineModel(this ProductViewModel productItem, int quantity)
        {
            return new PurchaseOrderLine
            {
                ProductId = productItem.ProductId,
                Quantity = quantity,
                IsPhysicalProduct = productItem.IsPhysicalProduct,
                ProductTypeCode = productItem.ProductTypeCode,
                UnitPrice = productItem.Price
            };
        }

        public static Data.Entities.PurchaseOrder ToCreateOrderModel(this List<PurchaseOrderLine> orderLines, int customerId)
        {
            var total = orderLines.Sum(item => item.UnitPrice * item.Quantity);

            var order = new Data.Entities.PurchaseOrder
            {
                PurchaseOrderNumber = "",
                CustomerId = customerId,
                Status = PurchaseOrderConstants.OrderPlaced,
                PurchaseOrderLines = orderLines,
                Total = total,
            };

            return order;
        }
    }
}
