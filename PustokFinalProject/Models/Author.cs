namespace PustokFinalProject.Models
{
    public class Author:BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<ProductAuthor> ProductAuthors { get; set; }
        public Author()
        {
            ProductAuthors = new HashSet<ProductAuthor>();
        }
    }
}
