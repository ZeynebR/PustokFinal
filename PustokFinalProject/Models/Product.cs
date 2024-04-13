using System.ComponentModel.DataAnnotations.Schema;

namespace PustokFinalProject.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal SellingPrice { get; set; } = default!;
        public decimal? DiscountedPrice { get; set; }
        public double? Rating { get; set; }
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
        [NotMapped]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        public IFormFile HoverFile { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public List<ProductImage> ProductImages { get; set; }
        public ICollection<ProductAuthor> ProductAuthors { get; set; }
        public ICollection<ProductTag>? ProductTags { get; set; }

        public Product()
        {
            ProductAuthors = new HashSet<ProductAuthor>();
            ProductTags = new HashSet<ProductTag>();
        }


    }
}

