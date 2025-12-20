using Domain.Enums;

namespace Application.Responses.Aluguel
{
    public class AluguelResponse
    {
        public int Id { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public decimal Valor { get; set; }
        public MetodoPagamentoEnum MetodoDePagamento { get; set; }
        public AluguelStatusesEnum Status { get; set; }
        public int InquilinoId { get; set; }
        public int ImovelId { get; set; }
    }
}
