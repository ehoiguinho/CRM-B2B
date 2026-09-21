using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set;} = null!;
    public DbSet<Contato> Contatos {get; set; } = null!;
    public DbSet<Lead> Leads { get; set; } = null!;
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

        modelBuilder.Entity<Contato>()
            .ToTable("tb_contato");

        modelBuilder.Entity<Contato>()
            .HasOne(c => c.Cliente)
            .WithMany(c => c.Contatos)
            .HasForeignKey(c => c.ClienteId);

        modelBuilder.Entity<Contato>()
            .Property(c => c.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Contato>()
            .Property(c => c.ClienteId)
            .HasColumnName("cliente_id");

        modelBuilder.Entity<Contato>()
            .Property(c => c.Nome)
            .HasColumnName("nome");

        modelBuilder.Entity<Contato>()
            .Property(c => c.Email)
            .HasColumnName("email");

        modelBuilder.Entity<Contato>()
            .Property(c => c.Telefone)
            .HasColumnName("telefone");

        modelBuilder.Entity<Contato>()
            .Property(c => c.Cargo)
            .HasColumnName("cargo");
            
        modelBuilder.Entity<Contato>()
            .Property(c => c.CriadoEm)
            .HasColumnName("criado_em");
        modelBuilder.Entity<Lead>()
        .ToTable("tb_lead");

        modelBuilder.Entity<Lead>()
            .Property(l => l.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Lead>()
            .Property(l => l.Nome)
            .HasColumnName("nome");

        modelBuilder.Entity<Lead>()
            .Property(l => l.Empresa)
            .HasColumnName("empresa");

        modelBuilder.Entity<Lead>()
            .Property(l => l.Email)
            .HasColumnName("email");

        modelBuilder.Entity<Lead>()
            .Property(l => l.Telefone)
            .HasColumnName("telefone");

        modelBuilder.Entity<Lead>()
            .Property(l => l.Origem)
            .HasColumnName("origem");

        modelBuilder.Entity<Lead>()
            .Property(l => l.Status)
            .HasColumnName("status")
            .HasDefaultValue("NOVO")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Lead>()
            .Property(l => l.Observacao)
            .HasColumnName("observacao");

        modelBuilder.Entity<Lead>()
            .Property(l => l.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }

}