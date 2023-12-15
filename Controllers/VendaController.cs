using Microsoft.AspNetCore.Mvc;
using ApiBrechoRamires.ViewModels.ResponseModels;
using ApiBrechoRamires.DTO;
using ApiBrechoRamires.ViewModels.Errors;
using ApiBrechoRamires.Services.Venda;
using ApiBrechoRamires.Models;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class VendaController : ControllerBase
{
    private readonly IVendaService _vendaService;
    private readonly ILogger<VendaController> _logger;

    public const uint pageNumber = 1;
    public const uint pageSize = 25;

    public VendaController(ILogger<VendaController> logger, IVendaService vendaService)
    {
        _logger = logger;
        _vendaService = vendaService;
    }

    /// <summary>
    /// Lista de Vendas Cadastradas
    /// </summary>
    /// <param name="pageNumber">pageNumber</param>
    /// <param name="pageSize">pageSize</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna a lista de vendas cadastradas.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpGet("GetVendasAsync")]
    [ProducesResponseType(typeof(RequestModel<ListModel<VendaDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ListModel<VendaDTO>>>> GetVendasAsync([FromQuery] uint pageNumber = pageNumber, [FromQuery] uint pageSize = pageSize)
    {
        try
        {
            var listaVendas = await _vendaService.GetVendasAsync(pageNumber, pageSize);
            var response = new RequestModel<ListModel<VendaDTO>>
            {
                Details = listaVendas,
                Status = (int)HttpStatusCode.OK,
                Error = null,
                TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.HResult, "Erro ao obter a lista de vendas.");
            return StatusCode(500, new ResultError
            (
                $"Erro interno do servidor: {ex.Message}",
                (short)HttpStatusCode.InternalServerError,
                DateTimeOffset.Now
            ));
        }
    }

    /// <summary>
    /// Venda pelo Id
    /// </summary>
    /// <param name="id">Id da venda</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna a venda pelo Id.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpGet("GetVendaByIdAsync/{id}")]
    [ProducesResponseType(typeof(RequestModel<VendaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<VendaDTO>>> GetVendaByIdAsync([FromRoute] string id)
    {
        try
        {
            var venda = await _vendaService.GetVendaByIdAsync(id);

            if (venda == null)
            {
                return NotFound(new ResultError($"Venda com código {id} não encontrada.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<VendaDTO>
            {
                Details = venda,
                Status = (int)HttpStatusCode.OK,
                Error = null,
                TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter a venda com código {id}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }

    /// <summary>
    /// Registrar uma venda
    /// </summary>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Registra uma venda.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpPost("PostVendaAsync")]
    [ProducesResponseType(typeof(RequestModel<ResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ResponseDTO>>> PostVendaAsync([FromBody] VendaModel model)
    {
        try
        {
            var venda = await _vendaService.PostVendaAsync(model);

            var response = new RequestModel<ResponseDTO>
            {
                Details = venda,
                Status = (int)HttpStatusCode.OK,
                Error = null,
                TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao registrar a venda.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }

    /// <summary>
    /// Apagar Venda pelo Id
    /// </summary>
    /// <param name="id">Código do produto</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna a venda deletada.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpDelete("DeleteVendaAsync/{id}")]
    [ProducesResponseType(typeof(RequestModel<ResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ResponseDTO>>> DeleteVendaAsync([FromRoute] string id)
    {
        try
        {
            var venda = await _vendaService.DeleteVendaAsync(id);

            if (venda == null)
            {
                return NotFound(new ResultError($"Venda com código {id} não encontrada.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<ResponseDTO>
            {
                Details = venda,
                Status = (int)HttpStatusCode.OK,
                Error = null,
                TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter a venda com código {id}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }

    /// <summary>
    /// Editar Venda pelo Id
    /// </summary>
    /// <param name="id">Id da venda</param>
    /// <param name="model">Modelo de dados do produto</param>
    /// <returns>Objeto JSON</returns>
    /// <response code="200">Retorna o produto deletado.</response>
    /// <response code="400">Se os dados forem Inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso negado.</response>
    /// <response code="417">Se os dados forem Inválidos.</response>
    /// <response code="500">Se houver algum problema com o servidor.</response>
    [HttpPut("EditVendaAsync/{id}")]
    [ProducesResponseType(typeof(RequestModel<ResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status417ExpectationFailed)]
    [ProducesResponseType(typeof(ResultError), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestModel<ResponseDTO>>> EditVendaAsync([FromRoute] string id, [FromBody] VendaModel model)
    {
        try
        {
            var venda = await _vendaService.EditVendaAsync(id, model);

            if (venda == null)
            {
                return NotFound(new ResultError($"Venda com código {id} não encontrada.", (short)HttpStatusCode.NotFound, DateTimeOffset.Now));
            }

            var response = new RequestModel<ResponseDTO>
            {
                Details = venda,
                Status = (int)HttpStatusCode.OK,
                Error = null,
                TimestampUtc = DateTimeOffset.Now
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao obter a venda com código {id}.");
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResultError($"Erro interno do servidor: {ex.Message}", (short)HttpStatusCode.InternalServerError, DateTimeOffset.Now));
        }
    }
}
