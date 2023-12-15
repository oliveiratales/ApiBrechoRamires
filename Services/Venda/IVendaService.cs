using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;
using ApiBrechoRamires.ViewModels.ResponseModels;

namespace ApiBrechoRamires.Services.Venda
{
    public interface IVendaService
    {

        #region GETS
        Task<ListModel<VendaDTO>> GetVendasAsync(uint pageNumber, uint pageSize);
        Task<VendaDTO?> GetVendaByIdAsync(string codigo);

        #endregion

        #region POST
        Task<ResponseDTO> PostVendaAsync(VendaModel model);

        #endregion

        #region DELETE
        Task<ResponseDTO?> DeleteVendaAsync(string codigo);

        #endregion

        #region PUT
        Task<ResponseDTO?> EditVendaAsync(string codigo, VendaModel model);

        #endregion
        
    }
}