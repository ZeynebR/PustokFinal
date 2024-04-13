namespace PustokFinalProject.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }

        public Category? ParentCategories { get; set; }
        public ICollection<Category>? Subcategories { get; set; }
    }
}


