using Domain.Enums;

namespace Domain.Entities;

public partial class Imovel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public TipoImovelEnum TipoImovel { get; set; }
    public string? Endereco { get; set; }
    public string? Descricao { get; set; }
    public bool? IsAtivo { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<Aluguel> Aluguels { get; set; } = new List<Aluguel>();

    public Imovel() { }

    public Imovel(string nome, TipoImovelEnum tipo)
    {
        Nome = nome;
        TipoImovel = tipo;
    }
    public Imovel(string nome, TipoImovelEnum tipoImovel, string? endereco, string? descricao)
    {
        Nome = nome;
        TipoImovel = tipoImovel;
        Endereco = endereco ?? null;
        Descricao = descricao ?? null;
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
}
