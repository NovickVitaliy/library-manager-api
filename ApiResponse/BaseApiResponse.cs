namespace library_manager_api.ApiResponse;

public abstract class BaseApiResponse
{
    public int StatusCode { get; set; }
    public string Description { get; set; } = string.Empty;
}