using Product.Microservice.Dtos.Product;

namespace Product.Microservice.Dtos.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public List<ProductDto> Products { get; set; } = [];
        public string? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastModifiedByName { get; set; }

    }
}
