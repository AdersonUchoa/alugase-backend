namespace Application.Responses.Aluguel
{
    public class AluguelDashboardResponse
    {
        public int TotalAlugueis { get; set; }
        public int AlugueisEmAndamento { get; set; }
        public int AlugueisConcluídos { get; set; }
        public int AlugueisCancelados { get; set; }
    }
}
