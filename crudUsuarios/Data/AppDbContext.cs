using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;

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
        }
}