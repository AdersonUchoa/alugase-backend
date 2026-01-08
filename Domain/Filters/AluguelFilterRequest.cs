using Domain.Enums;

namespace Domain.Filters
{
    public class AluguelFilterRequest
    {
        public DateOnly? PeriodoInicio { get; set; }
        public DateOnly? PeriodoFim { get; set; }
        public decimal? ValorMin { get; set; }
        public decimal? ValorMax { get; set; }
        public List<AluguelStatusesEnum>? Status { get; set; }
        public List<MetodoPagamentoEnum>? MetodosPagamento { get; set; }
        public PeriodoFiltroEnum? TipoPeriodo { get; set; } = PeriodoFiltroEnum.TodosPeriodos;
    }
}
