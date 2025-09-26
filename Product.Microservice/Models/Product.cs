using Microsoft.EntityFrameworkCore;
using Product.Microservice.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Product.Microservice.Models
{
    public class Product : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Precision(18, 2)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
    }
}
