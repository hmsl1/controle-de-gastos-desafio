using ExpenseControl.Api.Data;
using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Api.Controllers;


/// Endpoint de consulta de totais: totais por pessoa + total geral consolidado.

[ApiController]
[Route("api/totals")]
public class TotalsController : ControllerBase
{
    private readonly AppDbContext _db;

    public TotalsController(AppDbContext db)
    {
        _db = db;
    }

    /// GET /api/totals
    /// Para cada pessoa calcula: total de receitas, total de despesas e saldo (receita - despesa).
    /// Ao final, soma tudo para exibir o total geral de todas as pessoas.

    [HttpGet]
    public ActionResult<GeneralTotalsDto> GetTotals()
    {
        var people = _db.People
            .Include(p => p.Transactions)
            .OrderBy(p => p.Name)
            .ToList();

        var perPerson = people.Select(p =>
        {
            var totalReceitas = p.Transactions
                .Where(t => t.Type == TransactionType.Receita)
                .Sum(t => t.Amount);

            var totalDespesas = p.Transactions
                .Where(t => t.Type == TransactionType.Despesa)
                .Sum(t => t.Amount);

            return new PersonTotalsDto(p.Id, p.Name, totalReceitas, totalDespesas, totalReceitas - totalDespesas);
        }).ToList();

        var totalGeralReceitas = perPerson.Sum(p => p.TotalReceitas);
        var totalGeralDespesas = perPerson.Sum(p => p.TotalDespesas);

        var result = new GeneralTotalsDto(
            perPerson,
            totalGeralReceitas,
            totalGeralDespesas,
            totalGeralReceitas - totalGeralDespesas
        );

        return Ok(result);
    }
}
