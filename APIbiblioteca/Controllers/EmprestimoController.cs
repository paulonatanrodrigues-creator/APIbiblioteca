using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIbiblioteca.Models;

[Route("api/[controller]")]
[ApiController]
public class EmprestimoController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    public EmprestimoController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: api/Emprestimo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Emprestimo>>> GetEmprestimo()
    {
        return await _context.Emprestimos.ToListAsync();
    }

    // GET: api/Emprestimo/5
    [HttpGet("{emprestimoid}")]
    public async Task<ActionResult<Emprestimo>> GetEmprestimo(int emprestimoid)
    {
        var emprestimo = await _context.Emprestimos.FindAsync(emprestimoid);

        if (emprestimo == null)
        {
            return NotFound();
        }

        return emprestimo;
    }

    // PUT: api/Emprestimo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{emprestimoid}")]
    public async Task<IActionResult> PutEmprestimo(int? emprestimoid, Emprestimo emprestimo)
    {
        if (emprestimoid != emprestimo.EmprestimoId)
        {
            return BadRequest();
        }

        _context.Entry(emprestimo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmprestimoExists(emprestimoid))
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

    // POST: api/Emprestimo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Emprestimo>> PostEmprestimo(Emprestimo emprestimo)
    {
        _context.Emprestimos.Add(emprestimo);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEmprestimo", new { emprestimoid = emprestimo.EmprestimoId }, emprestimo);
    }

    // DELETE: api/Emprestimo/5
    [HttpDelete("{emprestimoid}")]
    public async Task<IActionResult> DeleteEmprestimo(int? emprestimoid)
    {
        var emprestimo = await _context.Emprestimos.FindAsync(emprestimoid);
        if (emprestimo == null)
        {
            return NotFound();
        }

        _context.Emprestimos.Remove(emprestimo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EmprestimoExists(int? emprestimoid)
    {
        return _context.Emprestimos.Any(e => e.EmprestimoId == emprestimoid);
    }
}
