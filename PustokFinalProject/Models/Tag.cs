namespace PustokFinalProject.Models
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<ProductTag> ProductTags { get; set; }
        public Tag()
        {
            ProductTags = new HashSet<ProductTag>();
        }
    }
}
