namespace Auth.Microservice.Services.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }
}
