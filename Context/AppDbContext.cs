using ApiBrechoRamires.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBrechoRamires.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<VendaModel> Vendas { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
    }
}
