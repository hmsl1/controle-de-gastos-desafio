namespace ExpenseControl.Api.Dtos;

/// Totais de uma pessoa específica.
public record PersonTotalsDto(Guid PersonId, string Name, decimal TotalReceitas, decimal TotalDespesas, decimal Saldo);

/// Totais de todas as pessoas + o total geral consolidado.
public record GeneralTotalsDto(
    List<PersonTotalsDto> Pessoas,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal SaldoGeral
);
