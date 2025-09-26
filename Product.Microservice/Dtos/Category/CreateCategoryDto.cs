using System.ComponentModel.DataAnnotations;

namespace Product.Microservice.Dtos.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
