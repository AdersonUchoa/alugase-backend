using Application.Responses.Aluguel;
using Domain.Enums;

namespace Application.Responses.Imovel
{
    public class ImovelResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public TipoImovelEnum TipoImovel { get; set; }
        public string? Endereco { get; set; }
        public string? Descricao { get; set; }
        public List<AluguelResponse>? Aluguels { get; set; }
        public int TotalAlugueis { get; set; }
        public int AlugueisAtivos { get; set; }
    }
}
