using System.Net;

namespace Infrastructure.Response;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; }


    public ApiResponse(T data)
    {
        StatusCode = 200;
        Data = data;
        Message = String.Empty;
    }
    public ApiResponse(HttpStatusCode statusCode, string message)
    {
        Data = default;
        StatusCode = (int)statusCode;
        Message = message;
    }
}