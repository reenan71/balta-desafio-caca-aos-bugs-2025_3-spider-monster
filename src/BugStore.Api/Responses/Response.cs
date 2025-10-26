using System.Text.Json.Serialization;

namespace BugStore.Api.Responses;

public class Response<TData>
{
    private readonly int _statusCode;

    [JsonConstructor]
    public Response()
    {
        _statusCode = 200;
    }

    public Response(TData? data, int code = 200, string? message = null)
    {
        _statusCode = code;
        Data = data;
        Message = message;
    }

    public TData? Data { get; set; }
    public string? Message { get; set; }
    
    [JsonIgnore]
    public int StatusCode => _statusCode;

    [JsonIgnore]
    public bool IsSuccess => _statusCode >= 200 && _statusCode < 300;
}