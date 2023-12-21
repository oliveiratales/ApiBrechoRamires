using System.Net.Http;
using ApiBrechoRamires.Context;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;
using ApiBrechoRamires.ViewModels.Errors;
using ApiBrechoRamires.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace ApiBrechoRamires.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;

        public LoginService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginDTO> GetLoginAsync(string email, string senha)
        {
            try 
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return null!;
                }

                bool senhaValida = VerificarSenha(senha, user.Senha);

                if (senhaValida)
                {
                    return new LoginDTO 
                    {
                        Id = user.Id,
                        Nome = user.Nome,
                        Email = user.Email,
                        Senha = user.Senha
                    };
                }
                else
                {
                    return new LoginDTO
                    {
                        Id = 0,
                        Nome = "",
                        Email = "",
                        Senha = ""
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }        
        }


        public async Task<ListModel<LoginDTO?>> GetUsersAsync(uint pageNumber, uint pageSize)
        {
            try
            {
                var users = await _context.Usuarios
                    .Skip((int)((pageNumber - 1) * pageSize))
                    .Take((int)pageSize)
                    .Select(user => new LoginDTO
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Nome = user.Nome,
                        Senha = user.Senha
                    }).ToListAsync();

                var totalRecords = await _context.Usuarios.CountAsync();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                var listModel = new ListModel<LoginDTO>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalNumberOfPages = totalPages,
                    TotalNumberOfRecords = totalRecords,
                    Results = users
                };

                return listModel!;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter os usuários.", ex);
            }
        }

        public async Task<ResponseDTO> PostUserAsync(LoginModel model)
        {
            try
            {
                var user = new LoginModel
                {
                    Email = model.Email,
                    Nome = model.Nome,
                    Senha = HashSenha(model.Senha)
                };

                _context.Usuarios.Add(user);

                await _context.SaveChangesAsync();

                var userId = user.Id;

                var responseDTO = new ResponseDTO
                {
                    Codigo = userId.ToString()!,
                    Mensagem = "Usuário registrado com sucesso!"
                };

                return responseDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao registrar o usuário.", ex);
            }
        }

        public async Task<ResponseDTO?> EditUserAsync(int id, LoginModel model)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id);

                user!.Nome = model.Nome;
                user.Senha = HashSenha(model.Senha);
                user.Email = model.Email;

                await _context.SaveChangesAsync();

                var updatedUserDTO = new ResponseDTO
                {
                    Codigo = $"{user.Id}",
                    Mensagem = "Usuário atualizado com sucesso."
                };

                return updatedUserDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar o usuário.", ex);
            }
        }

        public async Task<ResponseDTO?> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id);

                _context.Usuarios.Remove(user!);
                await _context.SaveChangesAsync();

                var deletedUserDTO = new ResponseDTO
                {
                    Codigo = $"{user!.Id}",
                    Mensagem = "Usuário excluído com sucesso!"
                };

                return deletedUserDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar o usuário.", ex);
            }
        }

        private bool VerificarSenha(string senhaDigitada, string senhaArmazenada)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaArmazenada);
        }

        private string HashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public class UnauthorizedAccessException : Exception
        {
            public UnauthorizedAccessException(string message) : base(message)
            {
            }
        }
    }
}