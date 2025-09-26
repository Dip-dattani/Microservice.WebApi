﻿using System.ComponentModel.DataAnnotations;

namespace Product.Microservice.Dtos.Product
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
