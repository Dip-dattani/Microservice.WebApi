using System.ComponentModel.DataAnnotations;

namespace Auth.Microservice.Models.Base
{
    public abstract class AuditableEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastModificationDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? DeletionTime { get; set; }

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
