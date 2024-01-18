using Saneej.FunBooksAndVideos.Service.Models;

namespace Saneej.FunBooksAndVideos.Service.Mappers
{
    public class PurchaseOrderMapper : IPurchaseOrderMapper
    {
        public PurchaseOrderResponse MapOrderDetailsFromEntity(Data.Entities.PurchaseOrder purchaseOrderEntity)
        {
            return new PurchaseOrderResponse
            {
                PurchaseOrderId = purchaseOrderEntity.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrderEntity.PurchaseOrderNumber,
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
    }
}
