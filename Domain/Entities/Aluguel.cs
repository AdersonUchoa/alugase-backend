using Domain.Enums;

namespace Domain.Entities;

public partial class Aluguel
{
    public int Id { get; set; }
    public DateOnly DataInicio { get; set; }
    public DateOnly DataFim { get; set; }
    public decimal Valor { get; set; }
    public MetodoPagamentoEnum MetodoDePagamento { get; set; }
    public AluguelStatusesEnum Status { get; set; }
    public int InquilinoId { get; set; }
    public int ImovelId { get; set; }
    public bool? IsAtivo { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual Imovel Imovel { get; set; } = null!;
    public virtual Inquilino Inquilino { get; set; } = null!;

    public Aluguel() { }

    public Aluguel(DateOnly dataInicio, DateOnly dataFim, decimal valor, MetodoPagamentoEnum metodoPagamento, AluguelStatusesEnum status, int inquilinoId, int imovelId)
    {
        DataInicio = dataInicio;
        DataFim = dataFim;
        Valor = valor;
        MetodoDePagamento = metodoPagamento; 
        Status = status;
        InquilinoId = inquilinoId;
        ImovelId = imovelId;
        CreatedAt = DateTime.Now;
        IsAtivo = true;
    }

    public void Ativar()
    {
        IsAtivo = true;
        UpdatedAt = DateTime.Now;
    }

    public void Desativar()
    {
        IsAtivo = false;
        UpdatedAt = DateTime.Now;
    }

    public void Cancelado()
    {
        Status = AluguelStatusesEnum.Cancelado;
        UpdatedAt = DateTime.Now;
    }
    public void PendenteDeEntrada()
    {
        Status = AluguelStatusesEnum.PendenteDeEntrada;
        UpdatedAt = DateTime.Now;
    }
    public void PendenteDePagamento()
    {
        Status = AluguelStatusesEnum.PendenteDePagamento;
        UpdatedAt = DateTime.Now;
    }
    public void Pausado()
    {
        Status = AluguelStatusesEnum.Pausado;
        UpdatedAt = DateTime.Now;
    }
    public void EmAndamento()
    {
        Status = AluguelStatusesEnum.EmAndamento;
        UpdatedAt = DateTime.Now;
    }
    public void Finalizado()
    {
        Status = AluguelStatusesEnum.Finalizado;
        UpdatedAt = DateTime.Now;
    }
}
