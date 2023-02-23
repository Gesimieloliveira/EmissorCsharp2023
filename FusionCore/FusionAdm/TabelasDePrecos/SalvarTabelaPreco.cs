using FusionCore.FusionAdm.Sessao;
using NHibernate;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class SalvarTabelaPreco
    {
        private ISession _sessao;
        private RepositorioTabelaPreco _repositorioTabelaPreco;

        public SalvarTabelaPreco()
        {
            _sessao = SessaoHelperFactory.AbrirSessaoAdm();
            _repositorioTabelaPreco = new RepositorioTabelaPreco(_sessao);
        }

        public void Salvar(TabelaPreco tabelaPreco)
        {
            using (_sessao)
            using (var transacao = _sessao.BeginTransaction())
            {
                _repositorioTabelaPreco.SalvaOuAtualiza(tabelaPreco);
                transacao.Commit();
            }
        }
    }
}