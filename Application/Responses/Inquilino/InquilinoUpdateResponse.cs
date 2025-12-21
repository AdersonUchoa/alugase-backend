namespace Application.Responses.Inquilino
{
    public class InquilinoUpdateResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Telefone { get; set; } = null!;
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
    }
}
