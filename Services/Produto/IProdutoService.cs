using ApiBrechoRamires.DTO;

namespace ApiBrechoRamires.Services.Produto
{
    public interface IProdutoService
    {

        #region Produtos
        Task<List<ProdutoDTO>> GetProdutosAsync();
        Task<ProdutoDTO?> GetProdutoByIdAsync(string codigo);

        #endregion
    }
}