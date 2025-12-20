namespace Application.Responses.Imovel
{
    public class ImovelDashboardResponse
    {
        public int TotalImoveis { get; set; }
        public int ImoveisComAlugueisAtivos { get; set; }
        public int ImoveisSemAlugueis { get; set; }
        public int ImoveisInativos { get; set; }

        public double PercentualComAlugueis => TotalImoveis > 0
            ? Math.Round((double)ImoveisComAlugueisAtivos / TotalImoveis * 100, 2)
            : 0;

        public double PercentualSemAlugueis => TotalImoveis > 0
            ? Math.Round((double)ImoveisSemAlugueis / TotalImoveis * 100, 2)
            : 0;
    }
}
