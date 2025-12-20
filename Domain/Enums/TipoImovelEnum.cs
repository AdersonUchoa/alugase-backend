using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoImovelEnum
    {
        [EnumMember(Value = "CASA")] Casa,
        [EnumMember(Value = "APARTAMENTO")] Apartamento,
    }
}