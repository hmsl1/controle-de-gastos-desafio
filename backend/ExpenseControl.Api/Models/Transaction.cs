namespace ExpenseControl.Api.Models;

/// Entidade que representa uma transação financeira (receita ou despesa) vinculada a uma pessoa.

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    /// Chave estrangeira para a pessoa dona da transação.
    public Guid PersonId { get; set; }

    /// Propriedade de navegação (EF Core usa para fazer o JOIN com Person).
    public Person? Person { get; set; }
}
