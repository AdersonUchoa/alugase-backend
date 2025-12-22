using System.ComponentModel.DataAnnotations;


namespace Application.Requests.Imovel
{
    public class CheckDisponibilidadeImovelRequest
    {
        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateOnly Data { get; set; }
    }
}
