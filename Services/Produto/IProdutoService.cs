using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;

namespace ApiBrechoRamires.Services.Produto
{
    public interface IProdutoService
    {

        #region GETS
        Task<List<ProdutoDTO>> GetProdutosAsync();
        Task<ProdutoDTO?> GetProdutoByIdAsync(string codigo);

        #endregion

        #region POST
        Task<PostProdutoDTO> PostProdutoAsync(ProdutoModel model);

        #endregion

        #region DELETE
        Task<PostProdutoDTO?> DeleteProdutoAsync(string codigo);

        #endregion

        #region PUT
        Task<PostProdutoDTO?> EditProdutoAsync(string codigo, ProdutoModel model);

        #endregion
    }
}