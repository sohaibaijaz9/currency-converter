namespace CurrencyConverterBackend.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }
        public T Data { get; set; }

        public Response()
        {
            Success = true;
        }
        public Response(string message = null)
        {
            Success = true;
            Message = message;
        }

        public Response(string message, bool success, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
