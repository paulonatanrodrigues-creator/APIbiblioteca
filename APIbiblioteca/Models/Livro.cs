using System;
using System.Collections.Generic;

namespace APIbiblioteca.Models;

public partial class Livro
{
    public int LivroId { get; set; }

    public string Titulo { get; set; } = null!;

    public int? AnoPublicacao { get; set; }

    public string? Isbn { get; set; }

    public int QuantidadeTotal { get; set; }

    public int QuantidadeDisponivel { get; set; }

    public int? AutorId { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Autore? Autor { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}
