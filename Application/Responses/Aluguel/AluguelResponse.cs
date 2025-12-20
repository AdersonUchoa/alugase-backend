using Application.Responses.Imovel;
using Application.Responses.Inquilino;

namespace Application.Responses.Aluguel
{
    public class AluguelResponse
    {
        public int Id { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public decimal Valor { get; set; }
        public required string MetodoDePagamento { get; set; }
        public required string Status { get; set; }
        public int InquilinoId { get; set; }
        public int ImovelId { get; set; }

        public InquilinoResumoResponse? Inquilino { get; set; }
        public ImovelResumoResponse? Imovel { get; set; }
    }
}
