# Controle de Gastos Residenciais

Projeto dividido em duas partes:
- `backend/ExpenseControl.Api` — API em .NET 8 (C#) com EF Core + SQLite (persistência em arquivo).
- `frontend` — React + TypeScript (Vite).

## Como rodar o back-end

Pré-requisito: [.NET 8 SDK](https://dotnet.microsoft.com/download) instalado.

bash
cd backend/ExpenseControl.Api
dotnet restore
dotnet run

No console vai aparecer algo como `Now listening on: http://localhost:5000`.
**Anote essa porta** — o front-end precisa dela.

O banco de dados (`expenses.db`) é criado automaticamente na primeira execução, na mesma
pasta do projeto. Ele não é apagado quando a aplicação é fechada.

Pode testar a API em `http://localhost:5000/swagger`.

## Como rodar o front-end

Pré-requisito: Node.js 18+.

bash
cd frontend
npm install
npm run dev

Abra o endereço que o Vite mostrar (geralmente `http://localhost:5173`).

⚠️ Se a porta do back-end que apareceu no `dotnet run` for diferente de `5000`,
ajuste a constante `API_URL` em `frontend/src/api.ts`.

## Regras de negócio implementadas

- Pessoa: Id (Guid gerado automaticamente), Nome, Idade. CRUD de criação/listagem/exclusão.
- Ao excluir uma pessoa, todas as suas transações são excluídas junto (cascade delete configurado no `AppDbContext`, ver `Data/AppDbContext.cs`).
- Transação: Id (Guid), Descrição, Valor, Tipo (Receita/Despesa), PessoaId.
- O `PessoaId` precisa existir (validado em `TransactionsController.Create`).
- Se a pessoa for menor de 18 anos, só pode cadastrar despesas — validado tanto no back-end (fonte da verdade, `TransactionsController`) quanto escondido no front-end para melhor UX (`TransactionsSection.tsx`).
- Totais: para cada pessoa soma receitas, despesas e calcula o saldo; ao final soma tudo para o total geral (`TotalsController.cs`).

## Estrutura do back-end

```
ExpenseControl.Api/
├── Models/           entidades (Person, Transaction, TransactionType)
├── Data/             AppDbContext (EF Core)
├── Dtos/             objetos de entrada/saída da API
├── Controllers/      endpoints REST (PeopleController, TransactionsController, TotalsController)
└── Program.cs        configuração da aplicação (DB, CORS, Swagger)
```
