using ApiBrechoRamires.Context;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;
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

        #region GETS
        public async Task<List<ProdutoDTO>> GetProdutosAsync() {
            try
            {
                var produtos = await _context.Produtos
                .Select(produto => new ProdutoDTO
                {
                    Codigo = produto.Codigo!,
                    Nome = produto.Nome,
                    Quantidade = produto.Quantidade,
                    Marca = produto.Marca,
                    Tipo = produto.Tipo,
                    Categoria = produto.Categoria,
                    Cor = produto.Cor,
                    Tamanho = produto.Tamanho,
                    PrecoPago = produto.PrecoPago,
                    Preco = produto.Preco,
                    Origem = produto.Origem,
                    Dono = produto.Dono
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
                    .Where(produto => produto.Codigo == codigo)
                    .Select(produto => new ProdutoDTO
                    {
                        Codigo = produto.Codigo!,
                        Nome = produto.Nome,
                        Quantidade = produto.Quantidade,
                        Marca = produto.Marca,
                        Tipo = produto.Tipo,
                        Categoria = produto.Categoria,
                        Cor = produto.Cor,
                        Tamanho = produto.Tamanho,
                        PrecoPago = produto.PrecoPago,
                        Preco = produto.Preco,
                        Origem = produto.Origem,
                        Dono = produto.Dono
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

        #endregion
    
        #region POST

        public async Task<PostProdutoDTO> PostProdutoAsync(ProdutoModel model)
        {
            try
            {
                var ultimoCodigo = await _context.Produtos
                    .Where(p => p.Tipo == model.Tipo)
                    .OrderByDescending(p => p.Codigo)
                    .Select(p => p.Codigo)
                    .FirstOrDefaultAsync();

                int proximoNumero = 1;
                if (!string.IsNullOrEmpty(ultimoCodigo) && int.TryParse(ultimoCodigo.Substring(1), out int numero))
                {
                    proximoNumero = numero + 1;
                }

                char tipoPrefixo = model.Tipo switch
                {
                    1 => 'V',
                    2 => 'C',
                    3 => 'A',
                    _ => throw new InvalidOperationException("Tipo de produto inválido."),
                };

                string novoCodigo;
                if (proximoNumero >= 1000)
                {
                    novoCodigo = $"{tipoPrefixo}{proximoNumero:D4}";
                }
                else
                {
                    novoCodigo = $"{tipoPrefixo}{proximoNumero:D3}";
                }
              
                var novoProduto = new ProdutoModel
                {
                    Codigo = novoCodigo,
                    Nome = model.Nome,
                    Quantidade = model.Quantidade,
                    Marca = model.Marca,
                    Tipo = model.Tipo,
                    Categoria = model.Categoria,
                    Cor = model.Cor,
                    Tamanho = model.Tamanho,
                    PrecoPago = model.PrecoPago,
                    Preco = model.Preco,
                    Origem = model.Origem,
                    Dono = model.Dono
                };

                _context.Produtos.Add(novoProduto);
                await _context.SaveChangesAsync();

                var novoProdutoId = novoProduto.Codigo;

                var postProdutoDTO = new PostProdutoDTO
                {
                    Codigo = novoProdutoId,
                    Mensagem = "Produto registrado com sucesso!"
                };

                return postProdutoDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar o produto.", ex);
            }
        }

        #endregion

        #region DELETE

        public async Task<PostProdutoDTO?> DeleteProdutoAsync(string codigo)
        {
            try
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p => p.Codigo == codigo);

                if (produto == null)
                {
                    return null;
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                var deletedProdutoDTO = new PostProdutoDTO
                {
                    Codigo = produto.Codigo!,
                    Mensagem = "Produto excluído com sucesso!"
                };

                return deletedProdutoDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar o produto.", ex);
            }
        }

        #endregion

        #region PUT

        public async Task<PostProdutoDTO?> EditProdutoAsync(string codigo, ProdutoModel model)
        {
            try
            {
                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(p => p.Codigo == codigo);

                if (produto == null)
                {
                    return null;
                }

                produto.Nome = model.Nome;
                produto.Quantidade = model.Quantidade;
                produto.Marca = model.Marca;
                produto.Tipo = model.Tipo;
                produto.Categoria = model.Categoria;
                produto.Cor = model.Cor;
                produto.Tamanho = model.Tamanho;
                produto.PrecoPago = model.PrecoPago;
                produto.Preco = model.Preco;
                produto.Origem = model.Origem;
                produto.Dono = model.Dono;

                await _context.SaveChangesAsync();

                var updatedProdutoDTO = new PostProdutoDTO
                {
                    Codigo = produto.Codigo!,
                    Mensagem = "Item atualizado com sucesso."
                };

                return updatedProdutoDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o produto.", ex);
            }
        }

        #endregion
    }   
}