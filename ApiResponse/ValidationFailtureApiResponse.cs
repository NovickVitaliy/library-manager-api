namespace library_manager_api.ApiResponse;

public class ValidationFailtureApiResponse : BaseApiResponse
{
    public IDictionary<string, List<string>> ValidationErrors { get; set; } = new Dictionary<string, List<string>>();
}