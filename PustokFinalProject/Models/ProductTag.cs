using PustokFinalProject.Areas.Admin.ViewModels;

namespace PustokFinalProject.Models
{
    public class ProductTag:BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public static explicit operator ProductTag(ProductTagVm productTagVm)
        {
            return new ProductTag
            {
                ProductId = productTagVm.ProductId,
               
                TagId = productTagVm.TagId
            };
        }
    }
}
