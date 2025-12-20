using System.ComponentModel.DataAnnotations;


namespace Application.Requests.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "O login é obrigatório.")]
        [StringLength(255, ErrorMessage = "O login deve ter no máximo 255 caracteres.")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(255, ErrorMessage = "A senha deve ter no máximo 255 caracteres.")]
        public string Senha { get; set; } = null!;
    }
}
