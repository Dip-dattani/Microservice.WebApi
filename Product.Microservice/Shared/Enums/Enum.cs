using System.ComponentModel;

namespace Product.Microservice.Shared.Enums
{
    public enum ResponseCodes
    {
        [Description("Invalid input provided")]
        InvalidInput,
        [Description("Failed to create resource")]
        FailedToCreate,
        [Description("Resource not found")]
        NotFound,
        [Description("Operation completed successfully")]
        Success
    }

    public enum StatusCodes
    {
        Status200OK = 200,
        Status201Created = 201,
        Status400BadRequest = 400,
        Status404NotFound = 404,
        Status401Unauthorized = 401
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
    }
}
