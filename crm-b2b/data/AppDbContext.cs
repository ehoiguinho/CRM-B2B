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
    public DbSet<Oportunidade> Oportunidades { get; set; } = null!;
    public DbSet<Atividade> Atividades { get; set; } = null!;
    public DbSet<Contrato> Contratos { get; set; } = null!;
    public DbSet<Fatura> Faturas { get; set; } = null!;
    public DbSet<Pagamento> Pagamentos { get; set; } = null!;
    public DbSet<ProdutoServico> ProdutosServicos { get; set; } = null!;
    public DbSet<OportunidadeItem> OportunidadeItens { get; set; } = null!;
    public DbSet<Inadimplencia> Inadimplencias { get; set; } = null!;
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

        modelBuilder.Entity<Oportunidade>()
        .ToTable("tb_oportunidade");

        modelBuilder.Entity<Oportunidade>()
            .Property(o => o.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Oportunidade>()
            .Property(o => o.ClienteId)
            .HasColumnName("cliente_id");

        modelBuilder.Entity<Oportunidade>()
            .Property(o => o.Titulo)
            .HasColumnName("titulo");

        modelBuilder.Entity<Oportunidade>()
            .Property(o => o.Descricao)
            .HasColumnName("descricao");

        modelBuilder.Entity<Oportunidade>()
            .Property(o => o.Valor)
            .HasColumnName("valor")
            .HasPrecision(15, 2);

        modelBuilder.Entity<Oportunidade>()
            .Property(o => o.Etapa)
            .HasColumnName("etapa")
            .HasDefaultValue("ABERTA")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Oportunidade>()
            .Property(o => o.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Atividade>()
        .ToTable("tb_atividade");

        modelBuilder.Entity<Atividade>()
            .Property(a => a.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Atividade>()
            .Property(a => a.OportunidadeId)
            .HasColumnName("oportunidade_id");

        modelBuilder.Entity<Atividade>()
            .Property(a => a.Tipo)
            .HasColumnName("tipo");

        modelBuilder.Entity<Atividade>()
            .Property(a => a.Titulo)
            .HasColumnName("titulo");

        modelBuilder.Entity<Atividade>()
            .Property(a => a.Descricao)
            .HasColumnName("descricao");

        modelBuilder.Entity<Atividade>()
            .Property(a => a.DataAgendada)
            .HasColumnName("data_agendada");

        modelBuilder.Entity<Atividade>()
            .Property(a => a.Status)
            .HasColumnName("status")
            .HasDefaultValue("PENDENTE")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Atividade>()
            .Property(a => a.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Atividade>()
            .Property(a => a.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true);

        modelBuilder.Entity<Atividade>()
            .HasOne(a => a.Oportunidade)
            .WithMany()
            .HasForeignKey(a => a.OportunidadeId);

        modelBuilder.Entity<Contrato>()
            .ToTable("tb_contrato");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.ClienteId)
            .HasColumnName("cliente_id");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.Numero)
            .HasColumnName("numero");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.Titulo)
            .HasColumnName("titulo");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.Descricao)
            .HasColumnName("descricao");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.Valor)
            .HasColumnName("valor")
            .HasPrecision(15, 2);

        modelBuilder.Entity<Contrato>()
            .Property(c => c.DataInicio)
            .HasColumnName("data_inicio");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.DataFim)
            .HasColumnName("data_fim");

        modelBuilder.Entity<Contrato>()
            .Property(c => c.Status)
            .HasColumnName("status")
            .HasDefaultValue("RASCUNHO")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Contrato>()
            .Property(c => c.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Fatura>()
            .ToTable("tb_fatura");

        modelBuilder.Entity<Fatura>()
            .Property(f => f.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Fatura>()
            .Property(f => f.ContratoId)
            .HasColumnName("contrato_id");

        modelBuilder.Entity<Fatura>()
            .Property(f => f.Numero)
            .HasColumnName("numero");

        modelBuilder.Entity<Fatura>()
            .Property(f => f.Descricao)
            .HasColumnName("descricao");

        modelBuilder.Entity<Fatura>()
            .Property(f => f.Valor)
            .HasColumnName("valor")
            .HasPrecision(15, 2);

        modelBuilder.Entity<Fatura>()
            .Property(f => f.DataEmissao)
            .HasColumnName("data_emissao")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Fatura>()
            .Property(f => f.DataVencimento)
            .HasColumnName("data_vencimento");

        modelBuilder.Entity<Fatura>()
            .Property(f => f.Status)
            .HasColumnName("status")
            .HasDefaultValue("PENDENTE")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Fatura>()
            .Property(f => f.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Fatura>()
            .HasOne(f => f.Contrato)
            .WithMany()
            .HasForeignKey(f => f.ContratoId);

        modelBuilder.Entity<Pagamento>()
            .ToTable("tb_pagamento");

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.FaturaId)
            .HasColumnName("fatura_id");

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.Valor)
            .HasColumnName("valor")
            .HasPrecision(15, 2);

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.DataPagamento)
            .HasColumnName("data_pagamento");

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.FormaPagamento)
            .HasColumnName("forma_pagamento");

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.Status)
            .HasColumnName("status")
            .HasDefaultValue("CONFIRMADO")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Pagamento>()
            .HasOne(p => p.Fatura)
            .WithMany()
            .HasForeignKey(p => p.FaturaId);

        modelBuilder.Entity<ProdutoServico>()
            .ToTable("tb_produto_servico");

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.Id)
            .HasColumnName("id");

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.Nome)
            .HasColumnName("nome");

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.Descricao)
            .HasColumnName("descricao");

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.Tipo)
            .HasColumnName("tipo")
            .HasDefaultValue("SERVICO")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.Valor)
            .HasColumnName("valor")
            .HasPrecision(15, 2);

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true);

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<OportunidadeItem>()
            .ToTable("tb_oportunidade_item");

        modelBuilder.Entity<OportunidadeItem>()
            .Property(i => i.Id)
            .HasColumnName("id");

        modelBuilder.Entity<OportunidadeItem>()
            .Property(i => i.OportunidadeId)
            .HasColumnName("oportunidade_id");

        modelBuilder.Entity<OportunidadeItem>()
            .Property(i => i.ProdutoServicoId)
            .HasColumnName("produto_servico_id");

        modelBuilder.Entity<OportunidadeItem>()
            .Property(i => i.Quantidade)
            .HasColumnName("quantidade");

        modelBuilder.Entity<OportunidadeItem>()
            .Property(i => i.ValorUnitario)
            .HasColumnName("valor_unitario")
            .HasPrecision(15, 2);

        modelBuilder.Entity<OportunidadeItem>()
            .HasOne(i => i.Oportunidade)
            .WithMany()
            .HasForeignKey(i => i.OportunidadeId);

        modelBuilder.Entity<OportunidadeItem>()
            .HasOne(i => i.ProdutoServico)
            .WithMany()
            .HasForeignKey(i => i.ProdutoServicoId);
        
        modelBuilder.Entity<Inadimplencia>()
            .ToTable("tb_inadimplencia");

        modelBuilder.Entity<Inadimplencia>()
            .Property(i => i.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Inadimplencia>()
            .Property(i => i.FaturaId)
            .HasColumnName("fatura_id");

        modelBuilder.Entity<Inadimplencia>()
            .Property(i => i.ValorEmAberto)
            .HasColumnName("valor_em_aberto")
            .HasPrecision(15, 2);

        modelBuilder.Entity<Inadimplencia>()
            .Property(i => i.DataInadimplencia)
            .HasColumnName("data_inadimplencia");

        modelBuilder.Entity<Inadimplencia>()
            .Property(i => i.Status)
            .HasColumnName("status")
            .HasDefaultValue("ABERTA")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Inadimplencia>()
            .Property(i => i.DataRegularizacao)
            .HasColumnName("data_regularizacao");

        modelBuilder.Entity<Inadimplencia>()
            .Property(i => i.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Inadimplencia>()
            .HasOne(i => i.Fatura)
            .WithMany()
            .HasForeignKey(i => i.FaturaId);
    }
    
}