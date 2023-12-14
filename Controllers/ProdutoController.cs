using Microsoft.AspNetCore.Mvc;
using ApiBrechoRamires.ViewModels.ResponseModels;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.ViewModels.Errors;
using ApiBrechoRamires.Services.Produto;
using System.Net;
using ApiBrechoRamires.Models;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly ILogger<ProdutoController> _logger;

    public const uint pageNumber = 1;
    public const uint pageSize = 25;

    public ProdutoController(ILogger<ProdutoController> logger, IProdutoService produtoService)
    {
        _logger = logger;
        _produtoService = produtoService;
    }

    /// <summary>
    /// Lista de Produtos Cadastrados
    /// </summary>
    /// <param name="pageNumber">pageNumber</param>
    /// <param name="pageSize">pageSize</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna a lista de produtos cadastrados.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpGet("produto/GetProdutosAsync")]
    [ProducesResponseType(typeof(RequestModel<ProdutoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ListModel<ProdutoDTO>>>> GetProdutosAsync([FromQuery] uint pageNumber = pageNumber, [FromQuery] uint pageSize = pageSize)
      {
         try
         {
            var listaProdutos = await _produtoService.GetProdutosAsync(pageNumber, pageSize);
            var response = new RequestModel<ListModel<ProdutoDTO>>
            {
               Details = listaProdutos,
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
    /// Produto pelo Código
    /// </summary>
    /// <param name="codigo">Código do produto</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna o produto pelo código.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpGet("produto/GetProdutoByIdAsync/{codigo}")]
    [ProducesResponseType(typeof(RequestModel<ProdutoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ProdutoDTO>>> GetProdutoByIdAsync([FromRoute] string codigo)
      {
        try
        {
            var produto = await _produtoService.GetProdutoByIdAsync(codigo);

            if (produto == null)
            {
                return NotFound(new ResultError($"Produto com código {codigo} não encontrado.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<ProdutoDTO>
            {
               Details = produto,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter o produto com código {codigo}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
      }

      /// <summary>
    /// Registrar um produto
    /// </summary>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Registra um novo produto.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpPost("produto/PostProdutoAsync")]
    [ProducesResponseType(typeof(RequestModel<PostProdutoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<PostProdutoDTO>>> PostProdutoAsync([FromBody] ProdutoModel model)
      {
        try
        {
            var produto = await _produtoService.PostProdutoAsync(model);

            var response = new RequestModel<PostProdutoDTO>
            {
               Details = produto,
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
    /// Apagar Produto pelo Código
    /// </summary>
    /// <param name="codigo">Código do produto</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna o produto deletado.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpDelete("produto/DeleteProdutoAsync/{codigo}")]
    [ProducesResponseType(typeof(RequestModel<PostProdutoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<PostProdutoDTO>>> DeleteProdutoAsync([FromRoute] string codigo)
      {
        try
        {
            var produto = await _produtoService.DeleteProdutoAsync(codigo);

            if (produto == null)
            {
                return NotFound(new ResultError($"Produto com código {codigo} não encontrado.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<PostProdutoDTO>
            {
               Details = produto,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter o produto com código {codigo}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }

    /// <summary>
    /// Editar Produto pelo Código
    /// </summary>
    /// <param name="codigo">Código do produto</param>
    /// <param name="model">Modelo de dados do produto</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna o produto deletado.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpPut("produto/EditProdutoAsync/{codigo}")]
    [ProducesResponseType(typeof(RequestModel<PostProdutoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<PostProdutoDTO>>> EditProdutoAsync([FromRoute] string codigo, [FromBody] ProdutoModel model)
      {
        try
        {
            var produto = await _produtoService.EditProdutoAsync(codigo, model);

            if (produto == null)
            {
                return NotFound(new ResultError($"Produto com código {codigo} não encontrado.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<PostProdutoDTO>
            {
               Details = produto,
               Status = (int)HttpStatusCode.OK,
               Error = null,
               TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter o produto com código {codigo}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }
}
