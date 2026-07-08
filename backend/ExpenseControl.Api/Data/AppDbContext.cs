using ExpenseControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Api.Data;


/// Contexto do Entity Framework Core: representa a "ponte" entre as classes C#
/// (Person, Transaction) e as tabelas do banco SQLite (arquivo expenses.db).
/// É esse arquivo .db que garante que os dados persistam após fechar a aplicação.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Regra de negócio: "ao deletar uma pessoa, todas as transações dela devem ser apagadas".
        // OnDelete(DeleteBehavior.Cascade) faz o próprio banco cuidar disso automaticamente
        // quando uma Person é removida.
        modelBuilder.Entity<Person>()
            .HasMany(p => p.Transactions)
            .WithOne(t => t.Person)
            .HasForeignKey(t => t.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18,2)");

        base.OnModelCreating(modelBuilder);
    }
}
