using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIbiblioteca.Models;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    public CategoriaController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: api/Categoria
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoria()
    {
        return await _context.Categorias.ToListAsync();
    }

    // GET: api/Categoria/5
    [HttpGet("{categoriaid}")]
    public async Task<ActionResult<Categoria>> GetCategoria(int categoriaid)
    {
        var categoria = await _context.Categorias.FindAsync(categoriaid);

        if (categoria == null)
        {
            return NotFound();
        }

        return categoria;
    }

    // PUT: api/Categoria/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{categoriaid}")]
    public async Task<IActionResult> PutCategoria(int? categoriaid, Categoria categoria)
    {
        if (categoriaid != categoria.CategoriaId)
        {
            return BadRequest();
        }

        _context.Entry(categoria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoriaExists(categoriaid))
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

    // POST: api/Categoria
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategoria", new { categoriaid = categoria.CategoriaId }, categoria);
    }

    // DELETE: api/Categoria/5
    [HttpDelete("{categoriaid}")]
    public async Task<IActionResult> DeleteCategoria(int? categoriaid)
    {
        var categoria = await _context.Categorias.FindAsync(categoriaid);
        if (categoria == null)
        {
            return NotFound();
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoriaExists(int? categoriaid)
    {
        return _context.Categorias.Any(e => e.CategoriaId == categoriaid);
    }
}
