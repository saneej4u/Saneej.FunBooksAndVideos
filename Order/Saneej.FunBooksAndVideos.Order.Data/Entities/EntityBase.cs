using System.ComponentModel.DataAnnotations;

namespace Saneej.FunBooksAndVideos.Data.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            CreatedBy = string.Empty;
            CreatedOn = DateTime.UtcNow;
            ModifiedBy = string.Empty;
            ModifiedOn = DateTime.UtcNow;
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
