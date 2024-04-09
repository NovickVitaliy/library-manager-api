using System.Text.Json;

namespace library_manager_api.ApiResponse;

public record ValidationFailtureApiResponse : BaseApiResponse
{
    public required IDictionary<string, List<string>> ValidationErrors { get; set; } = new Dictionary<string, List<string>>();
    public override string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}