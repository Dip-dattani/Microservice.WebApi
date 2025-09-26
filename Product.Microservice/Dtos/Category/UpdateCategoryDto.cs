using System.ComponentModel.DataAnnotations;

namespace Product.Microservice.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
