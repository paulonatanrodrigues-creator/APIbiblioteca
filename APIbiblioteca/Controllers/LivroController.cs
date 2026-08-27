using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIbiblioteca.Models;

[Route("api/[controller]")]
[ApiController]
public class LivroController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    public LivroController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: api/Livro
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Livro>>> GetLivro()
    {
        return await _context.Livros.ToListAsync();
    }

    // GET: api/Livro/5
    [HttpGet("{livroid}")]
    public async Task<ActionResult<Livro>> GetLivro(int livroid)
    {
        var livro = await _context.Livros.FindAsync(livroid);

        if (livro == null)
        {
            return NotFound();
        }

        return livro;
    }

    // PUT: api/Livro/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{livroid}")]
    public async Task<IActionResult> PutLivro(int? livroid, Livro livro)
    {
        if (livroid != livro.LivroId)
        {
            return BadRequest();
        }

        _context.Entry(livro).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LivroExists(livroid))
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

    // POST: api/Livro
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Livro>> PostLivro(Livro livro)
    {
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLivro", new { livroid = livro.LivroId }, livro);
    }

    // DELETE: api/Livro/5
    [HttpDelete("{livroid}")]
    public async Task<IActionResult> DeleteLivro(int? livroid)
    {
        var livro = await _context.Livros.FindAsync(livroid);
        if (livro == null)
        {
            return NotFound();
        }

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LivroExists(int? livroid)
    {
        return _context.Livros.Any(e => e.LivroId == livroid);
    }
}
