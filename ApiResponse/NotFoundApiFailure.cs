using System.Text.Json;

namespace library_manager_api.ApiResponse;

public record NotFoundApiFailure : BaseApiResponse
{
    public required string Id { get; init; } = default!;
    public override string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}