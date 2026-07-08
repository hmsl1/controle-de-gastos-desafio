using System.Text.Json.Serialization;
using ExpenseControl.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Faz o enum TransactionType ser serializado como texto ("Receita"/"Despesa")
// em vez de número (0/1), o que facilita muito o consumo pelo front-end.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Configura o EF Core para usar SQLite, salvando tudo no arquivo expenses.db
// (fica na pasta onde a API é executada). É isso que garante a persistência dos dados.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=expenses.db"));

// Permite que o front-end React (rodando em outra porta, ex: localhost:5173)
// consiga chamar essa API sem ser bloqueado pelo navegador (CORS).
const string CorsPolicy = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Garante que o banco e as tabelas existam ao subir a aplicação
// (sem precisar rodar migrations manualmente - simples e suficiente para este desafio).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(CorsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
