namespace Product.Microservice.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;        
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
