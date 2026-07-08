# Controle de Gastos Residenciais

Sistema de controle de gastos com cadastro de pessoas, cadastro de transações e consulta de totais.

Backend em C# (.NET 8) com banco SQLite. Frontend em React com TypeScript.

## Como rodar o backend

Precisa ter o .NET 8 SDK instalado.

```
cd backend/ExpenseControl.Api
dotnet restore
dotnet run
```

Vai aparecer no terminal o endereço que a API está rodando, algo tipo `http://localhost:5000`.

O banco de dados é criado sozinho na primeira vez que roda (arquivo `expenses.db`), então os dados continuam salvos mesmo se fechar a aplicação.

## Como rodar o frontend

Precisa ter o Node.js instalado.

```
cd frontend
npm install
npm run dev
```

Abre o endereço que aparecer no terminal (geralmente `http://localhost:5173`).

Se a porta do backend for diferente de 5162, precisa trocar no arquivo `frontend/src/api.ts`, na linha da constante `API_URL`.

## Regras implementadas

- Pessoa tem id (gerado automaticamente), nome e idade.
- Ao apagar uma pessoa, todas as transações dela também são apagadas.
- Transação tem id (gerado automaticamente), descrição, valor, tipo (receita ou despesa) e o id da pessoa.
- O id da pessoa informado na transação precisa existir no cadastro.
- Se a pessoa for menor de 18 anos, só pode cadastrar despesa, não receita.
- A consulta de totais mostra receita, despesa e saldo de cada pessoa, e no final o total somando todo mundo.
