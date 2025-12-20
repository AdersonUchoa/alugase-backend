namespace Application.Responses.Inquilino
{
    public class InquilinoDashboardResponse
    {
        public int TotalInquilinos { get; set; }
        public int InquilinosComAlugueisAtivos { get; set; }
        public int InquilinosSemAlugueis { get; set; }
        public int InquilinosInativos { get; set; }

        public double PercentualComAlugueis => TotalInquilinos > 0
            ? Math.Round((double)InquilinosComAlugueisAtivos / TotalInquilinos * 100, 2)
            : 0;

        public double PercentualSemAlugueis => TotalInquilinos > 0
            ? Math.Round((double)InquilinosSemAlugueis / TotalInquilinos * 100, 2)
            : 0;
    }
}
