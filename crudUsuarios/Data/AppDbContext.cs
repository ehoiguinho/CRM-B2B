using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set;} = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .ToTable("tb_usuario");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Nome)
            .HasColumnName("nome");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Email)
            .HasColumnName("email");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Senha)
            .HasColumnName("senha");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Telefone)
            .HasColumnName("telefone");
        
        modelBuilder.Entity<Usuario>()
            .Property(u => u.Perfil)
            .HasColumnName("perfil");    
        
        modelBuilder.Entity<Cliente>()
        .ToTable("tb_cliente");
        modelBuilder.Entity<Cliente>()
            .Property(c => c.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.RazaoSocial)
            .HasColumnName("razao_social");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.NomeFantasia)
            .HasColumnName("nome_fantasia");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Cnpj)
            .HasColumnName("cnpj");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Email)
            .HasColumnName("email");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Telefone)
            .HasColumnName("telefone");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Status)
            .HasColumnName("status")
            .HasDefaultValue("ATIVO")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Cliente>()
            .Property(c => c.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}