using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public partial class Administrador
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public bool? IsAtivo { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Administrador() { }

    public Administrador(string login, string senha)
    {
        Login = login;
        Senha = senha;
        IsAtivo = true;
        CreatedAt = DateTime.Now;
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
