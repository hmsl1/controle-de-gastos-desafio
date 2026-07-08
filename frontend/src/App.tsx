import { useEffect, useState, useCallback } from 'react';
import type { Person, Transaction, GeneralTotals } from './types';
import { api } from './api';
import PeopleSection from './components/PeopleSection';
import TransactionsSection from './components/TransactionsSection';
import TotalsSection from './components/TotalsSection';

/**
 * Componente raiz: busca os dados da API e distribui entre as 3 seções do desafio
 * (pessoas, transações, totais). Sempre que algo é criado/excluído, `reloadAll`
 * é chamado para manter as 3 seções sincronizadas (ex: excluir pessoa afeta totais).
 */
export default function App() {
  const [people, setPeople] = useState<Person[]>([]);
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [totals, setTotals] = useState<GeneralTotals | null>(null);
  const [loadError, setLoadError] = useState<string | null>(null);

  const reloadAll = useCallback(async () => {
    try {
      const [peopleData, transactionsData, totalsData] = await Promise.all([
        api.getPeople(),
        api.getTransactions(),
        api.getTotals(),
      ]);
      setPeople(peopleData);
      setTransactions(transactionsData);
      setTotals(totalsData);
      setLoadError(null);
    } catch (err) {
      setLoadError(
        err instanceof Error
          ? `Não foi possível conectar à API: ${err.message}`
          : 'Não foi possível conectar à API.'
      );
    }
  }, []);

  useEffect(() => {
    reloadAll();
  }, [reloadAll]);

  return (
    <main className="container">
      <h1>Controle de Gastos Residenciais</h1>

      {loadError && (
        <p className="error">
          {loadError} — confira se o back-end está rodando (dotnet run) e se a porta em
          src/api.ts está correta.
        </p>
      )}

      <PeopleSection people={people} onChanged={reloadAll} />
      <TransactionsSection people={people} transactions={transactions} onChanged={reloadAll} />
      <TotalsSection totals={totals} />
    </main>
  );
}
