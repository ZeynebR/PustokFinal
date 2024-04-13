namespace PustokFinalProject.Models
{
    public class ProductTag:BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
