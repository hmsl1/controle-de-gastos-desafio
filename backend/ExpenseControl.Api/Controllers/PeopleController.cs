using ExpenseControl.Api.Data;
using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Api.Controllers;


/// Endpoints de gerenciamento de pessoas: criar, listar e deletar.

[ApiController]
[Route("api/people")]
public class PeopleController : ControllerBase
{
    private readonly AppDbContext _db;

    public PeopleController(AppDbContext db)
    {
        _db = db;
    }

    /// GET /api/people - lista todas as pessoas cadastradas.
    [HttpGet]
    public ActionResult<IEnumerable<PersonResponseDto>> GetAll()
    {
        var people = _db.People
            .OrderBy(p => p.Name)
            .Select(p => new PersonResponseDto(p.Id, p.Name, p.Age))
            .ToList();

        return Ok(people);
    }

    /// POST /api/people - cadastra uma nova pessoa. O Id é gerado automaticamente.
    [HttpPost]
    public ActionResult<PersonResponseDto> Create(CreatePersonDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("O nome é obrigatório.");

        if (dto.Age < 0 || dto.Age > 130)
            return BadRequest("Idade inválida.");

        var person = new Person
        {
            Name = dto.Name.Trim(),
            Age = dto.Age
        };

        _db.People.Add(person);
        _db.SaveChanges();

        var response = new PersonResponseDto(person.Id, person.Name, person.Age);
        return CreatedAtAction(nameof(GetAll), new { id = person.Id }, response);
    }

    /// DELETE /api/people/{id} - remove uma pessoa.
    /// Regra de negócio: ao remover a pessoa, todas as transações dela são removidas junto
    /// (o cascade delete é configurado no AppDbContext, então basta remover a Person).

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var person = _db.People
            .Include(p => p.Transactions)
            .FirstOrDefault(p => p.Id == id);

        if (person is null)
            return NotFound($"Pessoa com id '{id}' não encontrada.");

        _db.People.Remove(person);
        _db.SaveChanges();

        return NoContent();
    }
}
