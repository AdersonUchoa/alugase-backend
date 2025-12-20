using Application.Responses.Aluguel;

namespace Application.Responses.Inquilino
{
    public class InquilinoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public List<AluguelResponse>? Aluguels { get; set; }
        public int TotalAlugueis { get; set; }
        public int AlugueisAtivos { get; set; }
    }
}
