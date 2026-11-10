using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

[JsonSerializable(typeof(WireSystemOneRequest))]
[JsonSerializable(typeof(SystemOneResponse))]
internal partial class SystemOneJsonContext : JsonSerializerContext
{
}
