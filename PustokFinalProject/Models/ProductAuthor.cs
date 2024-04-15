using PustokFinalProject.Areas.Admin.ViewModels;

namespace PustokFinalProject.Models
{
    public class ProductAuthor:BaseEntity
    {

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }


        public static explicit operator ProductAuthor(ProductAuthorVm productAuthorVm )
        {
            return new ProductAuthor
            {
                ProductId = productAuthorVm.ProductId,
           
                AuthorId = productAuthorVm.AuthorId
            };
        }
    }
}
