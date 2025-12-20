using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Enums
{    
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public enum AluguelStatusesEnum
    { 
        [EnumMember(Value = "CANCELADO")] Cancelado,
        [EnumMember(Value = "PENDENTE DE PAGAMENTO")] PendenteDePagamento,
        [EnumMember(Value = "PENDENTE DE ENTRADA")] PendenteDeEntrada, //Já está tudo pago, só falta o inquilino entrar no imóvel
        [EnumMember(Value = "PAUSADO")] Pausado,
        [EnumMember(Value = "EM ANDAMENTO")] EmAndamento,
        [EnumMember(Value = "FINALIZADO")] Finalizado,
    } 
}