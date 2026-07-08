// Tipos espelhando os DTOs do back-end (a API devolve o enum como string:
// "Receita" / "Despesa", graças ao JsonStringEnumConverter configurado no Program.cs).

export type TransactionType = 'Receita' | 'Despesa';

export interface Person {
  id: string;
  name: string;
  age: number;
}

export interface Transaction {
  id: string;
  description: string;
  amount: number;
  type: TransactionType;
  personId: string;
}

export interface PersonTotals {
  personId: string;
  name: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface GeneralTotals {
  pessoas: PersonTotals[];
  totalReceitas: number;
  totalDespesas: number;
  saldoGeral: number;
}
