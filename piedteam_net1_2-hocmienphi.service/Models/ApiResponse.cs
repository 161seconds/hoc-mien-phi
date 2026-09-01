namespace piedteam_net1_2_hocmienphi.service.Models;

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public required string Message { get; set; }
    public object? Data { get; set; }
    public object? Error { get; set; }
    public string? TraceId { get; set; }
    // moi request se duoc dinh danh 1 traceId
    // traceId dung de phan biet cac request voi nhau
    public DateTime TimestampUtc { get; set; }
}
public static class ResponseBuilder
{
    public static ApiResponse CreateSuccessResponse(object? data, string message, string? traceId = null)
    {
        return new ApiResponse()
        {
            IsSuccess = true,
            Message = message,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }

    public static ApiResponse CreateErrorResponse(object? errors, string message, string? traceId = null)
    {
        return new ApiResponse()
        {
            IsSuccess = true,
            Message = message,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }
}