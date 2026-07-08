import type { GeneralTotals } from '../types';

interface Props {
  totals: GeneralTotals | null;
}

/** Seção de consulta: totais por pessoa e o total geral ao final. */
export default function TotalsSection({ totals }: Props) {
  if (!totals) return null;

  return (
    <section>
      <h2>Totais</h2>
      <table>
        <thead>
          <tr>
            <th>Pessoa</th>
            <th>Receitas</th>
            <th>Despesas</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {totals.pessoas.map((p) => (
            <tr key={p.personId}>
              <td>{p.name}</td>
              <td>{p.totalReceitas.toFixed(2)}</td>
              <td>{p.totalDespesas.toFixed(2)}</td>
              <td>{p.saldo.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
        <tfoot>
          <tr>
            <td><strong>Total geral</strong></td>
            <td><strong>{totals.totalReceitas.toFixed(2)}</strong></td>
            <td><strong>{totals.totalDespesas.toFixed(2)}</strong></td>
            <td><strong>{totals.saldoGeral.toFixed(2)}</strong></td>
          </tr>
        </tfoot>
      </table>
    </section>
  );
}
