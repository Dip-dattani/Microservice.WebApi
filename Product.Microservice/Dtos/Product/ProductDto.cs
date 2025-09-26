namespace Product.Microservice.Dtos.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastModifiedByName { get; set; }
    }
}
