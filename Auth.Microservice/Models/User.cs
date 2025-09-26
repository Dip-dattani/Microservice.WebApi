using Auth.Microservice.Models.Base;
using Auth.Microservice.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Auth.Microservice.Models
{
    public class User : AuditableEntity
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }
        public UserRoleType UserRole { get; set; }
    }
}
