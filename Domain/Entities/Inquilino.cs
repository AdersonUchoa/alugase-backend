namespace Domain.Entities;

public partial class Inquilino
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public bool? IsAtivo { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<Aluguel> Aluguels { get; set; } = new List<Aluguel>();

    public Inquilino() { }

    public Inquilino(string nome, string telefone)
    {
        Nome = nome;
        Telefone = telefone;
        CreatedAt = DateTime.Now;
        IsAtivo = true;

    }
    
    public Inquilino(string nome, string telefone, string? cpf, string? email, string? endereco)
    {
        Nome = nome;
        Telefone = telefone;
        Cpf = cpf ?? null;
        Email = email ?? null;
        Endereco = endereco ?? null;
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
