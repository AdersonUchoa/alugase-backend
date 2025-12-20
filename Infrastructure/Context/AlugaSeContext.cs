using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Context;

public partial class AlugaSeContext : DbContext
{
    public AlugaSeContext()
    {
    }

    public AlugaSeContext(DbContextOptions<AlugaSeContext> options)
        : base(options)
    {
    } 

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Aluguel> Aluguels { get; set; }

    public virtual DbSet<Imovel> Imovels { get; set; }

    public virtual DbSet<Inquilino> Inquilinos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Administ__3214EC27AA024FCA");

            entity.ToTable("Administrador");

            entity.HasIndex(e => e.Login, "UQ__Administ__7838F272C6F82EC0").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.IsAtivo)
                .HasDefaultValue(true)
                .HasColumnName("is_ativo");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.Senha)
                .HasMaxLength(255)
                .HasColumnName("senha");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Aluguel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Aluguel__3214EC274CC8FF74");

            entity.ToTable("Aluguel");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DataFim).HasColumnName("dataFim");
            entity.Property(e => e.DataInicio).HasColumnName("dataInicio");
            entity.Property(e => e.ImovelId).HasColumnName("imovel_ID");
            entity.Property(e => e.InquilinoId).HasColumnName("inquilino_ID");
            entity.Property(e => e.IsAtivo)
                .HasDefaultValue(true)
                .HasColumnName("is_ativo");
            entity.Property(e => e.MetodoDePagamento)
                .HasConversion(new EnumToStringConverter<MetodoPagamentoEnum>())
                .HasMaxLength(50)
                .HasColumnName("metodoDePagamento");
            entity.Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<AluguelStatusesEnum>())
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor");

            entity.HasOne(d => d.Imovel).WithMany(p => p.Aluguels)
                .HasForeignKey(d => d.ImovelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Aluguel_Imovel");

            entity.HasOne(d => d.Inquilino).WithMany(p => p.Aluguels)
                .HasForeignKey(d => d.InquilinoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Aluguel_Inquilino");
        });

        modelBuilder.Entity<Imovel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Imovel__3214EC277CF301F6");

            entity.ToTable("Imovel");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Endereco)
                .HasMaxLength(500)
                .HasColumnName("endereco");
            entity.Property(e => e.IsAtivo)
                .HasDefaultValue(true)
                .HasColumnName("is_ativo");
            entity.Property(e => e.TipoImovel)
                .HasConversion(new EnumToStringConverter<TipoImovelEnum>())
                .HasMaxLength(50)
                .HasColumnName("tipoImovel");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Inquilino>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inquilin__3214EC27C8DB754B");

            entity.ToTable("Inquilino");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Endereco)
                .HasMaxLength(500)
                .HasColumnName("endereco");
            entity.Property(e => e.IsAtivo)
                .HasDefaultValue(true)
                .HasColumnName("is_ativo");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
