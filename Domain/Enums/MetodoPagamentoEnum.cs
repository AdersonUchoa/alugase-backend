using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MetodoPagamentoEnum
    {
        [EnumMember(Value = "PIX")] Pix,
        [EnumMember(Value = "CRÉDITO")] Credito,
        [EnumMember(Value = "DÉBITO")] Debito,
        [EnumMember(Value = "BOLETO")] Boleto,
    }
}