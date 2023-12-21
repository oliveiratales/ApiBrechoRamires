using ApiBrechoRamires.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBrechoRamires.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<VendaModel> Vendas { get; set; }
        public DbSet<VendaProduto> VendaProdutos { get; set; }
        public DbSet<LoginModel> Usuarios { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendaProduto>()
                .HasKey(vp => vp.Id);

            modelBuilder.Entity<VendaProduto>()
                .HasOne(vp => vp.Venda)
                .WithMany(v => v.VendaProdutos)
                .HasForeignKey(vp => vp.VendaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VendaProduto>()
                .HasOne(vp => vp.Produto)
                .WithMany(p => p.VendaProdutos)
                .HasForeignKey(vp => vp.ProdutoCodigo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
