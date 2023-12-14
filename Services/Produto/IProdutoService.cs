using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;
using ApiBrechoRamires.ViewModels.ResponseModels;

namespace ApiBrechoRamires.Services.Produto
{
    public interface IProdutoService
    {

        #region GETS
        Task<ListModel<ProdutoDTO>> GetProdutosAsync(uint pageNumber, uint pageSize);
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