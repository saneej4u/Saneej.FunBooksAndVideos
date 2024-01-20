using Saneej.FunBooksAndVideos.Data.Entities;
using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Mappers
{
    public class PurchaseOrderMapper : IPurchaseOrderMapper
    {
        public PurchaseOrderResponse MapToPurchaseOrderResponse(Data.Entities.PurchaseOrder purchaseOrderEntity)
        {
            return new PurchaseOrderResponse
            {
                PurchaseOrderId = purchaseOrderEntity.PurchaseOrderId,
                Status = purchaseOrderEntity.Status,
                Total = purchaseOrderEntity.Total,
                CustomerId = purchaseOrderEntity.CustomerId,
                PurchaseOrderLines = purchaseOrderEntity.PurchaseOrderLines.Select(ol => MapOrderDetailsFromEntity(ol)).ToList()
            };
        }

        public PurchaseOrderLineResponse MapOrderDetailsFromEntity(Data.Entities.PurchaseOrderLine purchaseOrderLineEntity)
        {
            return new PurchaseOrderLineResponse
            {

                PurchaseOrderId = purchaseOrderLineEntity.PurchaseOrderId,
                ProductId = purchaseOrderLineEntity.ProductId,
                ProductTypeCode = purchaseOrderLineEntity.ProductTypeCode,
                Quantity = purchaseOrderLineEntity.Quantity,
                UnitPrice = purchaseOrderLineEntity.UnitPrice,
                IsPhysicalProduct = purchaseOrderLineEntity.IsPhysicalProduct
            };
        }

        public PurchaseOrderLine MapToPurchaseOrderLine(ProductViewModel productItem, int quantity)
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

        public Data.Entities.PurchaseOrder MapToPurchaseOrder(List<PurchaseOrderLine> orderLines, string orderStatus, decimal orderTotal, int customerId)
        {
            var order = new Data.Entities.PurchaseOrder
            {
                CustomerId = customerId,
                Status = orderStatus,
                PurchaseOrderLines = orderLines,
                Total = orderTotal,
            };

            return order;
        }
    }
}
