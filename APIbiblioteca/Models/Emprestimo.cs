using System;
using System.Collections.Generic;

namespace APIbiblioteca.Models;

public partial class Emprestimo
{
    public int EmprestimoId { get; set; }

    public int UsuarioId { get; set; }

    public int LivroId { get; set; }

    public DateOnly DataEmprestimo { get; set; }

    public DateOnly DataDevolucaoPrevista { get; set; }

    public DateOnly? DataDevolucaoEfetiva { get; set; }

    public string? StatusEmprestimo { get; set; }

    public virtual Livro Livro { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
