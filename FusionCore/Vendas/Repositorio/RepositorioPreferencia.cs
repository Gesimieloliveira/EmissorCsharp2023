using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Faturamentos;
using NHibernate;

namespace FusionCore.Vendas.Repositorio
{
    public class RepositorioPreferencia : Repositorio<FaturamentoPreferencia, string>
    {
        public RepositorioPreferencia(ISession sessao) : base(sessao)
        {
        }

        public void Salva(FaturamentoPreferencia faturamentoPreferencia)
        {
            if (faturamentoPreferencia.Id == 0)
            {
                Sessao.Persist(faturamentoPreferencia);
                Sessao.Flush();
                return;
            }

            Sessao.Update(faturamentoPreferencia);
            Sessao.Flush();
        }

        public FaturamentoPreferencia GetPeloIdentificadorMaquina(string identificador)
        {
            var query = Sessao.QueryOver<FaturamentoPreferencia>()
                .Where(i => i.IdentificadorMaquina == identificador);

            return query.SingleOrDefault();
        }

        public bool PossuiPreferenciaParaMaquina(string identificador)
        {
            var query = Sessao.QueryOver<FaturamentoPreferencia>()
                .Where(i => i.IdentificadorMaquina == identificador);

            return query.RowCount() > 0;
        }
    }
}