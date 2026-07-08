namespace ExpenseControl.Api.Dtos;

// DTOs usados para não expor a entidade do banco diretamente
// na API. Isso separa "o que o banco guarda" de "o que a API recebe/devolve".

/// Dados recebidos ao criar uma pessoa.
public record CreatePersonDto(string Name, int Age);

/// Dados devolvidos ao consultar uma pessoa.
public record PersonResponseDto(Guid Id, string Name, int Age);
