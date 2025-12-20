using System.ComponentModel.DataAnnotations;


namespace Application.Requests.Inquilino
{
    public class UpdateInquilinoRequest
    {
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
        public string? Nome { get; set; }

        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        public string? Telefone { get; set; }

        [StringLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$", ErrorMessage = "CPF inválido. Use o formato: 000.000.000-00 ou 00000000000")]
        public string? Cpf { get; set; }

        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(255, ErrorMessage = "O e-mail deve ter no máximo 255 caracteres.")]
        public string? Email { get; set; }

        [StringLength(500, ErrorMessage = "O endereço deve ter no máximo 500 caracteres.")]
        public string? Endereco { get; set; }
    }
}
