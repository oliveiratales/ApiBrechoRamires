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
                        Produtos = venda.ProdutosVendidos!.Select(produto => new ProdutoDTO
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
                        }).ToList() ?? new List<ProdutoDTO>()
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
            catch (Exception)
            {
                throw;
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
                        Produtos = venda.ProdutosVendidos!.Select(produto => new ProdutoDTO
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
                        }).ToList() ?? new List<ProdutoDTO>()
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
                    .FirstOrDefaultAsync(v => v.Id.ToString() == codigo);

                if (venda == null)
                {
                    return null;
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
                    .Include(v => v.ProdutosVendidos)
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

                venda.ProdutosVendidos = await CarregarDetalhesProdutosAsync(venda.ListaProdutos);

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

        private async Task<List<ProdutoModel>> CarregarDetalhesProdutosAsync(List<string>? listaProdutos)
        {
            if (listaProdutos == null || listaProdutos.Count == 0)
            {
                return new List<ProdutoModel>();
            }

            var produtos = await _context.Produtos
                .Where(p => listaProdutos.Contains(p.Codigo!))
                .ToListAsync();

            return produtos;
        }

    }
}
