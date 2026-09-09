using System.Net;
using System.Text.Json.Serialization;

namespace NoteApplication.API.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    [JsonConverter(typeof(JsonNumberEnumConverter<HttpStatusCode>))]
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static Result<T> Ok(T data, HttpStatusCode statusCode = HttpStatusCode.OK, string? message = null)
    {
        return new Result<T>
        {
            Data = data,
            IsSuccess = true,
            StatusCode = statusCode,
            Message = message
        };
    }

    public static Result<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>
        {
            Data = default,
            IsSuccess = false,
            StatusCode = statusCode,
            Message =  message 
        };
    }
}