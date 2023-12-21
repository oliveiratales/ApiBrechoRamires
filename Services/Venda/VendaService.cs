using ApiBrechoRamires.Context;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;
using ApiBrechoRamires.ViewModels.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace ApiBrechoRamires.Services.Venda
{
    public class VendaService : IVendaService
    {
        private readonly AppDbContext _context;

        public VendaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ListModel<VendaDTO>> GetVendasAsync(uint pageNumber, uint pageSize)
        {
            try
            {
                var vendas = await _context.Vendas
                    .Include(v => v.VendaProdutos!)
                    .ThenInclude(vp => vp.Produto!)
                    .Skip((int)((pageNumber - 1) * pageSize))
                    .Take((int)pageSize)
                    .Select(venda => new VendaDTO
                    {
                        Id = venda.Id,
                        DataVenda = venda.DataVenda,
                        Valor = venda.Valor,
                        Desconto = venda.Desconto,
                        FormaDePagamento = venda.FormaDePagamento,
                        Cliente = venda.Cliente,
                        Vendedor = venda.Vendedor,
                        Produtos = venda.VendaProdutos!.Select(vp => new ProdutoVendidoDTO
                        {
                            Codigo = vp.Produto!.Codigo!,
                            Nome = vp.Produto.Nome,
                            Quantidade = vp.Quantidade,
                            Marca = vp.Produto.Marca,
                            Tipo = vp.Produto.Tipo,
                            Categoria = vp.Produto.Categoria,
                            Cor = vp.Produto.Cor,
                            Tamanho = vp.Produto.Tamanho,
                            PrecoPago = vp.Produto.PrecoPago,
                            Preco = vp.Produto.Preco,
                            Origem = vp.Produto.Origem,
                            Dono = vp.Produto.Dono
                        }).ToList() ?? new List<ProdutoVendidoDTO>()
                    })
                    .ToListAsync();

                var totalRecords = await _context.Vendas.CountAsync();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                var listModel = new ListModel<VendaDTO>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalNumberOfPages = totalPages,
                    TotalNumberOfRecords = totalRecords,
                    Results = vendas
                };

                return listModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter as vendas.", ex);
            }
        }

        public async Task<VendaDTO?> GetVendaByIdAsync(string codigo)
        {
            try
            {
                var venda = await _context.Vendas
                    .Where(venda => venda.Id.ToString() == codigo)
                    .Select(venda => new VendaDTO
                    {
                        Id = venda.Id,
                        DataVenda = venda.DataVenda,
                        Valor = venda.Valor,
                        Desconto = venda.Desconto,
                        FormaDePagamento = venda.FormaDePagamento,
                        Cliente = venda.Cliente,
                        Vendedor = venda.Vendedor,
                        Produtos = venda.VendaProdutos!.Select(vp => new ProdutoVendidoDTO
                        {
                            Codigo = vp.Produto!.Codigo!,
                            Nome = vp.Produto.Nome,
                            Quantidade = vp.Quantidade,
                            Marca = vp.Produto.Marca,
                            Tipo = vp.Produto.Tipo,
                            Categoria = vp.Produto.Categoria,
                            Cor = vp.Produto.Cor,
                            Tamanho = vp.Produto.Tamanho,
                            PrecoPago = vp.Produto.PrecoPago,
                            Preco = vp.Produto.Preco,
                            Origem = vp.Produto.Origem,
                            Dono = vp.Produto.Dono
                        }).ToList() ?? new List<ProdutoVendidoDTO>()
                    })
                    .FirstOrDefaultAsync();

                return venda;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseDTO> PostVendaAsync(VendaModel model)
        {
            try
            {
                var novaVenda = new VendaModel
                {
                    DataVenda = DateTime.UtcNow,
                    Valor = model.Valor,
                    Desconto = model.Desconto,
                    FormaDePagamento = model.FormaDePagamento,
                    Cliente = model.Cliente,
                    Vendedor = model.Vendedor,
                    ListaProdutos = model.ListaProdutos
                };

                novaVenda.ProdutosVendidos = await CarregarDetalhesProdutosAsync(novaVenda.ListaProdutos);

                _context.Vendas.Add(novaVenda);

                foreach (var produtoVendido in novaVenda.ProdutosVendidos)
                {
                    var vendaProduto = new VendaProduto
                    {
                        ProdutoCodigo = produtoVendido.Codigo,
                        Produto = produtoVendido,
                        Venda = novaVenda,
                        Quantidade = novaVenda.ListaProdutos!
                            .FirstOrDefault(p => p.Codigo == produtoVendido.Codigo)
                            ?.Quantidade ?? 0
                    };

                    var produto = await _context.Produtos
                        .FirstOrDefaultAsync(p => p.Codigo == produtoVendido.Codigo);

                    if (produto != null)
                    {
                        produto.Quantidade -= vendaProduto.Quantidade;
                    }

                    _context.VendaProdutos.Add(vendaProduto);
                }

                await _context.SaveChangesAsync();

                var novaVendaId = novaVenda.Id;

                var responseDTO = new ResponseDTO
                {
                    Codigo = novaVendaId.ToString(),
                    Mensagem = "Venda registrada com sucesso!"
                };

                return responseDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar a venda.", ex);
            }
        }

        public async Task<ResponseDTO?> DeleteVendaAsync(string codigo)
        {
            try
            {
                var venda = await _context.Vendas
                    .Include(v => v.VendaProdutos)
                    .FirstOrDefaultAsync(v => v.Id.ToString() == codigo);

                if (venda == null)
                {
                    return null;
                }

                foreach (var vendaProduto in venda.VendaProdutos!)
                {
                    var produto = await _context.Produtos
                        .FirstOrDefaultAsync(p => p.Codigo == vendaProduto.ProdutoCodigo);

                    if (produto != null)
                    {
                        produto.Quantidade += vendaProduto.Quantidade;
                        produto.VendasAssociadas?.Remove(vendaProduto);                      
                    }
                }

                _context.Vendas.Remove(venda);

                await _context.SaveChangesAsync();

                var deletedVendaDTO = new ResponseDTO
                {
                    Codigo = venda.Id.ToString(),
                    Mensagem = "Venda excluída com sucesso!"
                };

                return deletedVendaDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar a venda.", ex);
            }
        }

        public async Task<ResponseDTO?> EditVendaAsync(string codigo, VendaModel model)
        {
            try
            {
                var venda = await _context.Vendas
                    .Include(v => v.VendaProdutos)
                    .FirstOrDefaultAsync(v => v.Id.ToString() == codigo);

                if (venda == null)
                {
                    return null;
                }

                venda.DataVenda = DateTime.UtcNow;
                venda.Valor = model.Valor;
                venda.Desconto = model.Desconto;
                venda.FormaDePagamento = model.FormaDePagamento;
                venda.Cliente = model.Cliente;
                venda.Vendedor = model.Vendedor;
                venda.ListaProdutos = model.ListaProdutos;

                _context.VendaProdutos.RemoveRange(venda.VendaProdutos!);

                venda.ProdutosVendidos = await CarregarDetalhesProdutosAsync(venda.ListaProdutos);

                foreach (var produtoVendido in venda.ProdutosVendidos)
                {
                    var produto = await _context.Produtos
                        .FirstOrDefaultAsync(p => p.Codigo == produtoVendido.Codigo);

                    var qtdVenda = venda.VendaProdutos!.FirstOrDefault(p => p.ProdutoCodigo == produto!.Codigo)!.Quantidade;
                    
                    produto!.Quantidade += qtdVenda;
                    
                    var vendaProduto = new VendaProduto
                    {
                        ProdutoCodigo = produtoVendido.Codigo,
                        Produto = produtoVendido,
                        Venda = venda,
                        Quantidade = venda.ListaProdutos!
                            .FirstOrDefault(p => p.Codigo == produtoVendido.Codigo)
                            ?.Quantidade ?? 0
                    };
                    
                    if (produto != null)
                    {
                        produto.Quantidade -= vendaProduto.Quantidade;                     
                    }

                    _context.VendaProdutos.Add(vendaProduto);
                }

                await _context.SaveChangesAsync();

                var updatedVendaDTO = new ResponseDTO
                {
                    Codigo = venda.Id.ToString(),
                    Mensagem = "Venda atualizada com sucesso."
                };

                return updatedVendaDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a venda.", ex);
            }
        }

        private async Task<List<ProdutoModel>> CarregarDetalhesProdutosAsync(List<ProdutoQuantidadeDTO>? listaProdutos)
        {
            if (listaProdutos == null || listaProdutos.Count == 0)
            {
                return new List<ProdutoModel>();
            }

            var codigosProdutos = listaProdutos.Select(produto => produto.Codigo).ToList();

            var produtos = await _context.Produtos
                .Where(p => codigosProdutos.Contains(p.Codigo!))
                .ToListAsync();

            return produtos;
        }

    }
}
