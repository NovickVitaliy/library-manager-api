namespace library_manager_api.ApiResponse;

public abstract record BaseApiResponse
{
    public required int StatusCode { get; init; }
    public required string Description { get; init; } = string.Empty;

    public abstract string ToJson();
}