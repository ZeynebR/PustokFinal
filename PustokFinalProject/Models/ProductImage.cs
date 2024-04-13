using System.ComponentModel.DataAnnotations.Schema;

namespace PustokFinalProject.Models
{
    public class ProductImage:BaseEntity
    {
        public string Url { get; set; } = null!;
        [NotMapped]
        public IFormFile File { get; set; } = null!;
        public bool IsMainCover { get; set; }
        public bool IsHovered { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
