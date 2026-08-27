using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIbiblioteca.Models;

[Route("api/[controller]")]
[ApiController]
public class AutorController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    public AutorController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: api/Autore
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Autore>>> GetAutore()
    {
        return await _context.Autores.ToListAsync();
    }

    // GET: api/Autore/5
    [HttpGet("{autorid}")]
    public async Task<ActionResult<Autore>> GetAutore(int autorid)
    {
        var autore = await _context.Autores.FindAsync(autorid);

        if (autore == null)
        {
            return NotFound();
        }

        return autore;
    }

    // PUT: api/Autore/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{autorid}")]
    public async Task<IActionResult> PutAutore(int? autorid, Autore autore)
    {
        if (autorid != autore.AutorId)
        {
            return BadRequest();
        }

        _context.Entry(autore).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AutoreExists(autorid))
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

    // POST: api/Autore
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Autore>> PostAutore(Autore autore)
    {
        _context.Autores.Add(autore);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAutore", new { autorid = autore.AutorId }, autore);
    }

    // DELETE: api/Autore/5
    [HttpDelete("{autorid}")]
    public async Task<IActionResult> DeleteAutore(int? autorid)
    {
        var autore = await _context.Autores.FindAsync(autorid);
        if (autore == null)
        {
            return NotFound();
        }

        _context.Autores.Remove(autore);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AutoreExists(int? autorid)
    {
        return _context.Autores.Any(e => e.AutorId == autorid);
    }
}
