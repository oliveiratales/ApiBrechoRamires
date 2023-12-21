using System.Collections.Specialized;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;
using ApiBrechoRamires.ViewModels.ResponseModels;

namespace ApiBrechoRamires.Services.Login
{
    public interface ILoginService
    {
        #region GETS
        Task<LoginDTO> GetLoginAsync(string email, string senha);
        Task<ListModel<LoginDTO?>> GetUsersAsync(uint pageNumber, uint pageSize);

        #endregion

        #region POST
        Task<ResponseDTO> PostUserAsync(LoginModel model);

        #endregion

        #region DELETE
        Task<ResponseDTO?> DeleteUserAsync(int id);

        #endregion

        #region PUT
        Task<ResponseDTO?> EditUserAsync(int id, LoginModel model);

        #endregion
    }
}