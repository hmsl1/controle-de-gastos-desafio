using ExpenseControl.Api.Data;
using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.Api.Controllers;

/// Endpoints de gerenciamento de transações: criar e listar (edição/deleção não são exigidas).

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public TransactionsController(AppDbContext db)
    {
        _db = db;
    }

    /// GET /api/transactions - lista todas as transações cadastradas.
    [HttpGet]
    public ActionResult<IEnumerable<TransactionResponseDto>> GetAll()
    {
        var transactions = _db.Transactions
            .Select(t => new TransactionResponseDto(t.Id, t.Description, t.Amount, t.Type, t.PersonId))
            .ToList();

        return Ok(transactions);
    }

    /// POST /api/transactions - cadastra uma nova transação.
    /// Regras de negócio aplicadas aqui:
    ///   1. A pessoa informada (PersonId) precisa existir.
    ///   2. Se a pessoa for menor de 18 anos, só pode cadastrar despesas (não receitas).

    [HttpPost]
    public ActionResult<TransactionResponseDto> Create(CreateTransactionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Description))
            return BadRequest("A descrição é obrigatória.");

        if (dto.Amount <= 0)
            return BadRequest("O valor da transação deve ser maior que zero.");

        var person = _db.People.Find(dto.PersonId);
        if (person is null)
            return BadRequest($"Não existe pessoa cadastrada com o id '{dto.PersonId}'.");

        // Regra de negócio central do desafio: menor de idade não pode ter receita.
        if (person.IsMinor && dto.Type == TransactionType.Receita)
            return BadRequest("Pessoas menores de 18 anos só podem cadastrar despesas, não receitas.");

        var transaction = new Transaction
        {
            Description = dto.Description.Trim(),
            Amount = dto.Amount,
            Type = dto.Type,
            PersonId = dto.PersonId
        };

        _db.Transactions.Add(transaction);
        _db.SaveChanges();

        var response = new TransactionResponseDto(
            transaction.Id, transaction.Description, transaction.Amount, transaction.Type, transaction.PersonId);

        return CreatedAtAction(nameof(GetAll), new { id = transaction.Id }, response);
    }
}
