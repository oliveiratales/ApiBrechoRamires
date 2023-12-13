using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiBrechoRamires.Models;
using ApiBrechoRamires.Context;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutoController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Produto
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoModel>>> GetProdutos()
    {
        return await _context.Produtos.ToListAsync();
    }

    // GET: api/Produto/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoModel>> GetProduto(string id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
        {
            return NotFound();
        }

        return produto;
    }

    // POST: api/Produto
    [HttpPost]
    public async Task<ActionResult<ProdutoModel>> PostProduto(ProdutoModel produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduto", new { codigo = produto.Codigo }, produto);
    }

    // PUT: api/Produto/5
    [HttpPut("{codigo}")]
    public async Task<IActionResult> PutProduto(string codigo, ProdutoModel produto)
    {
        if (codigo != produto.Codigo)
        {
            return BadRequest();
        }

        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Produtos.Any(e => e.Codigo == codigo))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Produto/5
    [HttpDelete("{codigo}")]
    public async Task<IActionResult> DeleteProduto(string codigo)
    {
        var produto = await _context.Produtos.FindAsync(codigo);

        if (produto == null)
        {
            return NotFound();
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
