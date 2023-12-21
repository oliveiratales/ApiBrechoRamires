using System.Net;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.Models;
using ApiBrechoRamires.Services.Login;
using ApiBrechoRamires.ViewModels.Errors;
using ApiBrechoRamires.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiBrechoRamires.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
    private readonly ILoginService _loginService;
    private readonly ILogger<UsuarioController> _logger;

    public const uint pageNumber = 1;
    public const uint pageSize = 25;

    public UsuarioController(ILogger<UsuarioController> logger, ILoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="senha">Senha</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna a autenticação do sistema.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpGet("produto/LoginAsync")]
    [ProducesResponseType(typeof(RequestModel<LoginDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<LoginDTO>>> LoginAsync([FromHeader] string email, [FromHeader] string senha)
      {
         try
         {
            var login = await _loginService.GetLoginAsync(email, senha);

            if (login == null)
            {
                return NotFound(new ResultError($"Usuário com código não encontrado.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            if (login.Id == 0)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas");
            }

            var response = new RequestModel<LoginDTO>
            {
               Details = login,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
         }
         catch (UnauthorizedAccessException ex)
         {
            return Unauthorized(new ResultError(ex.Message, (short)HttpStatusCode.Unauthorized, DateTimeOffset.Now));
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.HResult, "Erro ao realizar o login.");
            return StatusCode(500, new ResultError
            (
               $"Erro interno do servidor: {ex.Message}",
               (short)HttpStatusCode.InternalServerError,
               DateTimeOffset.Now
            ));
         }
    }

    /// <summary>
    /// Lista de usuário
    /// </summary>
    /// <param name="pageNumber">pageNumber</param>
    /// <param name="pageSize">pageSize</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna a lista de usuários cadastrados.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpGet("produto/GetUsersAsync")]
    [ProducesResponseType(typeof(RequestModel<ProdutoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ListModel<LoginDTO>>>> GetUserAsync([FromQuery] uint pageNumber = pageNumber, [FromQuery] uint pageSize = pageSize)
      {
         try
         {
            var users = await _loginService.GetUsersAsync(pageNumber, pageSize);
            var response = new RequestModel<ListModel<LoginDTO?>>
            {
               Details = users,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex.HResult, "Erro ao obter a lista de produtos.");
            return StatusCode(500, new ResultError
            (
               $"Erro interno do servidor: {ex.Message}",
               (short)HttpStatusCode.InternalServerError,
               DateTimeOffset.Now
            ));
         }
    }

    /// <summary>
    /// Registrar um usuário
    /// </summary>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Registra um novo usuário.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpPost("produto/PostUserAsync")]
    [ProducesResponseType(typeof(RequestModel<ResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ResponseDTO>>> PostUserAsync([FromBody] LoginModel model)
      {
        try
        {
            var user = await _loginService.PostUserAsync(model);

            var response = new RequestModel<ResponseDTO>
            {
               Details = user,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao registrar o produto.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
      }

    /// <summary>
    /// Apagar usuário pelo código
    /// </summary>
    /// <param name="id">Código do usuário</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna o produto deletado.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpDelete("produto/DeleteUserAsync/{id}")]
    [ProducesResponseType(typeof(RequestModel<ResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ResponseDTO>>> DeleteUserAsync([FromRoute] int id)
      {
        try
        {
            var user = await _loginService.DeleteUserAsync(id);

            if (user == null)
            {
                return NotFound(new ResultError($"Usuário com código {id} não encontrado.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<ResponseDTO>
            {
               Details = user,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter o usuário com código {id}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }

    /// <summary>
    /// Editar usuário pelo código
    /// </summary>
    /// <param name="id">Código do usuário</param>
    /// <param name="model">Modelo de dados do usuário</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna o produto deletado.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpPut("produto/EditUserAsync/{id}")]
    [ProducesResponseType(typeof(RequestModel<ResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ResponseDTO>>> EditUserAsync([FromRoute] int id, [FromBody] LoginModel model)
      {
        try
        {
            var user = await _loginService.EditUserAsync(id, model);

            if (user == null)
            {
                return NotFound(new ResultError($"Usuário com código {id} não encontrado.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<ResponseDTO>
            {
               Details = user,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter o usuário com código {id}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }
    }
}