namespace Product.Microservice.Dtos
{
    public class ResponseDto<T>
    {
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public T Result { get; set; } = default!;
    }
}
