using System;
using System.Collections.Generic;

namespace APIbiblioteca.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
