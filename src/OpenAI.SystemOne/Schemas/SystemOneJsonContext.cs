using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(WireSystemOneRequest))]
[JsonSerializable(typeof(SystemOneResponse))]
internal partial class SystemOneJsonContext : JsonSerializerContext
{
}
