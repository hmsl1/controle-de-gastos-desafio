import type { Person, Transaction, GeneralTotals, TransactionType } from './types';

// Ajuste essa porta conforme a que aparecer no console quando rodar `dotnet run`
// (procure por algo como "Now listening on: http://localhost:5162").
const API_URL = 'http://localhost:5000/api';

/** Função utilitária: chama o fetch e já trata erro devolvendo uma mensagem legível. */
async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });

  if (!response.ok) {
    const message = await response.text();
    throw new Error(message || `Erro ${response.status} ao chamar ${path}`);
  }

  // DELETE não retorna corpo (204 No Content)
  if (response.status === 204) return undefined as T;

  return response.json() as Promise<T>;
}

export const api = {
  // ---- Pessoas ----
  getPeople: () => request<Person[]>('/people'),

  createPerson: (name: string, age: number) =>
    request<Person>('/people', {
      method: 'POST',
      body: JSON.stringify({ name, age }),
    }),

  deletePerson: (id: string) =>
    request<void>(`/people/${id}`, { method: 'DELETE' }),

  // ---- Transações ----
  getTransactions: () => request<Transaction[]>('/transactions'),

  createTransaction: (description: string, amount: number, type: TransactionType, personId: string) =>
    request<Transaction>('/transactions', {
      method: 'POST',
      body: JSON.stringify({ description, amount, type, personId }),
    }),

  // ---- Totais ----
  getTotals: () => request<GeneralTotals>('/totals'),
};
