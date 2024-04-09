using System.Text.Json;

namespace library_manager_api.ApiResponse;

public record ValidationFailtureApiResponse : BaseApiResponse
{
    public IDictionary<string, List<string>> ValidationErrors { get; set; } = new Dictionary<string, List<string>>();
}