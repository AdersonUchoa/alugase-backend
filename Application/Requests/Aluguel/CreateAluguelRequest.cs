using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Aluguel
{
    public class CreateAluguelRequest : IValidatableObject
    {
        [Required(ErrorMessage = "A data de inicio é obrigatória.")]
        public DateOnly DataInicio { get; set; } 
        
        [Required(ErrorMessage = "A data de saída é obrigatória.")]
        public DateOnly DataFim { get; set; }

        [Required(ErrorMessage = "O valor do aluguel é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O método de pagamento é obrigatório.")]
        [EnumDataType(typeof(MetodoPagamentoEnum), ErrorMessage = "Método de pagamento inválido.")]
        public MetodoPagamentoEnum MetodoDePagamento { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [EnumDataType(typeof(AluguelStatusesEnum), ErrorMessage = "Status do aluguel inválido.")]
        public AluguelStatusesEnum Status { get; set; }

        [Required(ErrorMessage = "O ID do inquilino é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do inquilino deve ser maior que zero.")]
        public int InquilinoId { get; set; }
        
        [Required(ErrorMessage = "O ID do imóvel é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do imóvel deve ser maior que zero.")]
        public int ImovelId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataFim < DataInicio)
            {
                yield return new ValidationResult(
                    "A data de saída não pode ser anterior à data de início.",
                    new[] { nameof(DataFim) }
                );
            }
        }
    }
}
