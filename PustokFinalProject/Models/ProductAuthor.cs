namespace PustokFinalProject.Models
{
    public class ProductAuthor:BaseEntity
    {

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }
}
