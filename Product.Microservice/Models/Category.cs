using Product.Microservice.Models.Base;

namespace Product.Microservice.Models
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
