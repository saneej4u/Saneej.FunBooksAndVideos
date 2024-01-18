using System.ComponentModel.DataAnnotations;

namespace Saneej.FunBooksAndVideos.Data.Entities
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

        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public string ModifiedBy { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }
    }
}
