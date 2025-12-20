using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Aluguel
{
    public class UpdateAluguelRequest : IValidatableObject
    {
        public DateOnly? DataInicio { get; set; }

        public DateOnly? DataFim { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal? Valor { get; set; }

        [EnumDataType(typeof(MetodoPagamentoEnum), ErrorMessage = "Método de pagamento inválido.")]
        public MetodoPagamentoEnum? MetodoDePagamento { get; set; }

        [EnumDataType(typeof(AluguelStatusesEnum), ErrorMessage = "Status do aluguel inválido.")]
        public AluguelStatusesEnum? Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O ID do inquilino deve ser maior que zero.")]
        public int? InquilinoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O ID do imóvel deve ser maior que zero.")]
        public int? ImovelId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataInicio.HasValue && DataFim.HasValue)
            {
                if (DataFim.Value < DataInicio.Value)
                {
                    yield return new ValidationResult(
                        "A data de fim não pode ser anterior à data de início.",
                        new[] { nameof(DataFim) }
                    );
                }
            }
        }
    }
}
