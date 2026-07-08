namespace ExpenseControl.Api.Models;

/// Representa o tipo de uma transação financeira.
/// Uma transação só pode ser Receita (entrada de dinheiro) ou Despesa (saída de dinheiro).
public enum TransactionType
{
    Receita,
    Despesa
}
