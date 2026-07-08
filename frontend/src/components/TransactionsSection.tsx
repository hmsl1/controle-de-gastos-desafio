import { useState } from 'react';
import type { Person, Transaction, TransactionType } from '../types';
import { api } from '../api';

interface Props {
  people: Person[];
  transactions: Transaction[];
  onChanged: () => void;
}

/**
 * Seção de cadastro de transações: formulário de criação + lista.
 * A validação de "menor de idade só pode despesa" é feita no back-end (fonte da verdade),
 * mas aqui já escondemos a opção "Receita" no select quando a pessoa é menor,
 * para melhorar a experiência do usuário (evita erro antes mesmo de tentar enviar).
 */
export default function TransactionsSection({ people, transactions, onChanged }: Props) {
  const [description, setDescription] = useState('');
  const [amount, setAmount] = useState('');
  const [type, setType] = useState<TransactionType>('Despesa');
  const [personId, setPersonId] = useState('');
  const [error, setError] = useState<string | null>(null);

  const selectedPerson = people.find((p) => p.id === personId);
  const isMinorSelected = selectedPerson ? selectedPerson.age < 18 : false;

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    try {
      await api.createTransaction(description.trim(), Number(amount), type, personId);
      setDescription('');
      setAmount('');
      onChanged();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao cadastrar transação.');
    }
  }

  function personName(id: string) {
    return people.find((p) => p.id === id)?.name ?? '(pessoa removida)';
  }

  return (
    <section>
      <h2>Transações</h2>

      <form onSubmit={handleSubmit} className="form-row">
        <select value={personId} onChange={(e) => setPersonId(e.target.value)} required>
          <option value="" disabled>Selecione a pessoa</option>
          {people.map((p) => (
            <option key={p.id} value={p.id}>{p.name}</option>
          ))}
        </select>

        <input
          placeholder="Descrição"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
        />

        <input
          type="number"
          step="0.01"
          placeholder="Valor"
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
          min={0.01}
          required
        />

        <select value={type} onChange={(e) => setType(e.target.value as TransactionType)}>
          <option value="Despesa">Despesa</option>
          {!isMinorSelected && <option value="Receita">Receita</option>}
        </select>

        <button type="submit" disabled={people.length === 0}>Cadastrar</button>
      </form>

      {people.length === 0 && <p>Cadastre uma pessoa antes de lançar transações.</p>}
      {error && <p className="error">{error}</p>}

      <table>
        <thead>
          <tr>
            <th>Pessoa</th>
            <th>Descrição</th>
            <th>Tipo</th>
            <th>Valor</th>
          </tr>
        </thead>
        <tbody>
          {transactions.map((t) => (
            <tr key={t.id}>
              <td>{personName(t.personId)}</td>
              <td>{t.description}</td>
              <td>{t.type}</td>
              <td>{t.amount.toFixed(2)}</td>
            </tr>
          ))}
          {transactions.length === 0 && (
            <tr><td colSpan={4}>Nenhuma transação cadastrada ainda.</td></tr>
          )}
        </tbody>
      </table>
    </section>
  );
}
