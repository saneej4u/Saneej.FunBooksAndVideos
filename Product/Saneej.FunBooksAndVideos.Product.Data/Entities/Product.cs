namespace Saneej.FunBooksAndVideos.Product.Data.Entities
{
    public class Product : EntityBase
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductTypeId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public ProductType ProductType { get; set; }
    }
}
