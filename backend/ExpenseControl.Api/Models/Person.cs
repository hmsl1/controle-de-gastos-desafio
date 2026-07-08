namespace ExpenseControl.Api.Models;

/// Entidade que representa uma pessoa cadastrada no sistema.
/// Cada pessoa pode ter várias transações associadas (1 pessoa -> N transações).

public class Person
{
    /// Identificador único, gerado automaticamente no momento da criação.
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    /// Transações pertencentes a esta pessoa (relacionamento de navegação do EF Core).
    public List<Transaction> Transactions { get; set; } = new();

    /// Regra de negócio: pessoa é considerada menor de idade quando tem menos de 18 anos.
    /// Propriedade calculada (não é salva no banco), só para deixar a regra centralizada
    /// em um único lugar em vez de espalhar "age < 18" pelo código.
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsMinor => Age < 18;
}
