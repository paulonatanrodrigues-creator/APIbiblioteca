using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIbiblioteca.Models;

public partial class BibliotecaDbContext : DbContext
{
    public BibliotecaDbContext()
    {
    }

    public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autore> Autores { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Emprestimo> Emprestimos { get; set; }

    public virtual DbSet<Livro> Livros { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autore>(entity =>
        {
            entity.HasKey(e => e.AutorId);

            entity.Property(e => e.AutorId).HasColumnName("AutorID");
            entity.Property(e => e.Nacionalidade)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Emprestimo>(entity =>
        {
            entity.Property(e => e.EmprestimoId).HasColumnName("EmprestimoID");
            entity.Property(e => e.DataEmprestimo).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LivroId).HasColumnName("LivroID");
            entity.Property(e => e.StatusEmprestimo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Pendente");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Livro).WithMany(p => p.Emprestimos)
                .HasForeignKey(d => d.LivroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Emprestimos_Livros");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Emprestimos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Emprestimos_Usuarios");
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasIndex(e => e.Isbn, "UQ__Livros__447D36EAE57D0FA5").IsUnique();

            entity.Property(e => e.LivroId).HasColumnName("LivroID");
            entity.Property(e => e.AutorId).HasColumnName("AutorID");
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.QuantidadeDisponivel).HasDefaultValue(1);
            entity.Property(e => e.QuantidadeTotal).HasDefaultValue(1);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Autor).WithMany(p => p.Livros)
                .HasForeignKey(d => d.AutorId)
                .HasConstraintName("FK_Livros_Autores");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Livros)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK_Livros_Categorias");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105342FE2280A").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusConta)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Ativo");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
