namespace Saneej.FunBooksAndVideos.Product.Data.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            CreatedBy = string.Empty;
            CreatedOn = new DateTime(1800, 1, 1);
            ModifiedBy = string.Empty;
            ModifiedOn = new DateTime(1800, 1, 1);
        }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
