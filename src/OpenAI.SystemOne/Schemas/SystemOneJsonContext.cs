using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(WireSystemOneRequest))]
[JsonSerializable(typeof(SystemOneResponse))]
[JsonSerializable(typeof(SystemOneKeepAlive))]
[JsonSerializable(typeof(SystemOneContent))]
[JsonSerializable(typeof(SystemOneState))]
internal partial class SystemOneJsonContext : JsonSerializerContext
{
}
