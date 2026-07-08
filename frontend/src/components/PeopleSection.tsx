import { useState } from 'react';
import type { Person } from '../types';
import { api } from '../api';

interface Props {
  people: Person[];
  onChanged: () => void; // avisa o App para recarregar os dados (pessoas/transações/totais)
}

/**
 * Seção de cadastro de pessoas: formulário de criação + lista com opção de exclusão.
 * Ao excluir uma pessoa, o back-end já apaga as transações dela (cascade delete),
 * então aqui só precisamos chamar o delete e recarregar tudo.
 */
export default function PeopleSection({ people, onChanged }: Props) {
  const [name, setName] = useState('');
  const [age, setAge] = useState('');
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    try {
      await api.createPerson(name.trim(), Number(age));
      setName('');
      setAge('');
      onChanged();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao cadastrar pessoa.');
    }
  }

  async function handleDelete(id: string) {
    if (!confirm('Excluir esta pessoa também apagará todas as transações dela. Continuar?')) return;
    try {
      await api.deletePerson(id);
      onChanged();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao excluir pessoa.');
    }
  }

  return (
    <section>
      <h2>Pessoas</h2>

      <form onSubmit={handleSubmit} className="form-row">
        <input
          placeholder="Nome"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
        <input
          type="number"
          placeholder="Idade"
          value={age}
          onChange={(e) => setAge(e.target.value)}
          min={0}
          required
        />
        <button type="submit">Cadastrar</button>
      </form>

      {error && <p className="error">{error}</p>}

      <table>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Idade</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {people.map((p) => (
            <tr key={p.id}>
              <td>{p.name}</td>
              <td>{p.age}{p.age < 18 ? ' (menor)' : ''}</td>
              <td>
                <button onClick={() => handleDelete(p.id)}>Excluir</button>
              </td>
            </tr>
          ))}
          {people.length === 0 && (
            <tr><td colSpan={3}>Nenhuma pessoa cadastrada ainda.</td></tr>
          )}
        </tbody>
      </table>
    </section>
  );
}
