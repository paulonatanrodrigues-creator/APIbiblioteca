using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIbiblioteca.Models;

public partial class Autore
{
    [Key]
    public int AutorId { get; set; }

    public string Nome { get; set; } = null!;

    public string? Nacionalidade { get; set; }

    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
}
