using System.Net;

namespace Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public object Message { get; set; }
        public object Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public int? TotalPages { get; set; }
        public string? Details { get; set; }

        public ApiResponse(bool success, HttpStatusCode statusCode, object data, object message, int? totalPages, string? details)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Data = data;
            TotalPages = totalPages;
            Details = details;
        }
    }
}
