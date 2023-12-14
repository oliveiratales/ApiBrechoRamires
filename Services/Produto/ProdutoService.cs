using ApiBrechoRamires.Context;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Services.Produto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiBrechoRamires.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly AppDbContext _context;

        public ProdutoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProdutoDTO>> GetProdutosAsync() {
            try
            {
                var produtos = await _context.Produtos
                .Select(produto => new ProdutoDTO
                {
                    Codigo = produto.Codigo,
                    Nome = produto.Nome,
                    Quantidade = produto.Quantidade,
                    Marca = produto.Marca,
                    Categoria = produto.Categoria,
                    Cor = produto.Cor,
                    Tamanho = produto.Tamanho,
                    PrecoPago = produto.PrecoPago,
                    Preco = produto.Preco,
                    Origem = produto.Origem
                })
                .ToListAsync();

            return produtos;
            }
            catch (Exception)
            {
                throw;
            }
        }
    
        public async Task<ProdutoDTO?> GetProdutoByIdAsync(string codigo)
        {
            try
            {
                var produto = await _context.Produtos
                    .Where(p => p.Codigo == codigo)
                    .Select(p => new ProdutoDTO
                    {
                        Codigo = p.Codigo,
                        Nome = p.Nome,
                        Quantidade = p.Quantidade,
                        Marca = p.Marca,
                        Categoria = p.Categoria,
                        Cor = p.Cor,
                        Tamanho = p.Tamanho,
                        PrecoPago = p.PrecoPago,
                        Preco = p.Preco,
                        Origem = p.Origem
                    })
                    .FirstOrDefaultAsync();

                if (produto == null)
                {
                    return null;
                }

                return produto;  
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}