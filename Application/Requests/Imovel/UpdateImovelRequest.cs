using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Imovel
{
    public class UpdateImovelRequest
    {
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
        public string? Nome { get; set; }

        [EnumDataType(typeof(TipoImovelEnum), ErrorMessage = "Tipo de imóvel inválido.")]
        public TipoImovelEnum? TipoImovel { get; set; }

        [StringLength(500, ErrorMessage = "O endereço deve ter no máximo 500 caracteres.")]
        public string? Endereco { get; set; }

        public string? Descricao { get; set; }
    }
}
