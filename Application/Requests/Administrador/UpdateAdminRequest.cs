using System.ComponentModel.DataAnnotations;


namespace Application.Requests.Administrador
{
    public class UpdateAdminRequest
    {
        [StringLength(255, ErrorMessage = "O login deve ter no máximo 255 caracteres.")]
        public string? Login { get; set; }

        [StringLength(255, ErrorMessage = "A senha deve ter no máximo 255 caracteres.")]
        public string? Senha { get; set; }
    }
}
