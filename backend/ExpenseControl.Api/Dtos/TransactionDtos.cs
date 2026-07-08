using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Dtos;

/// Dados recebidos ao criar uma transação.
public record CreateTransactionDto(string Description, decimal Amount, TransactionType Type, Guid PersonId);

/// Dados devolvidos ao consultar uma transação.
public record TransactionResponseDto(Guid Id, string Description, decimal Amount, TransactionType Type, Guid PersonId);
