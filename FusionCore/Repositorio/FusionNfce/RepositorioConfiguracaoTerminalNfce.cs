using System.Collections.Generic;
using System.Text;
using FusionCore.FusionNfce.ConfiguracaoTerminal;
using FusionCore.FusionNfce.EmissorFiscal;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioConfiguracaoTerminalNfce
    {
        private readonly ISession _sessao;

        public RepositorioConfiguracaoTerminalNfce(ISession sessao)
        {
            _sessao = sessao;
        }

        public void Salvar(ConfiguracaoTerminalNfce configuracaoTerminal)
        {
            _sessao.SaveOrUpdate(configuracaoTerminal);
            _sessao.Flush();
        }

        public ConfiguracaoTerminalNfce GetPeloId(byte id)
        {
            var configuracao = _sessao.Get<ConfiguracaoTerminalNfce>(id);

            if (configuracao == null) return null;

            var idTerminal = configuracao.TerminalOfflineId;

            configuracao.EmissorFiscalLista = _sessao.QueryOver<NfceEmissorFiscal>()
                .Where(x => x.TerminalOfflineId == idTerminal).List();

            return configuracao;
        }

        public int ObterIntervaloSincronizacao()
        {
            var hql = new StringBuilder("select ");
            hql.Append("c.IntervaloSync as IntervaloSync ");
            hql.Append("from ConfiguracaoTerminalNfce as c");

            var query = _sessao.CreateQuery(hql.ToString())
                .SetResultTransformer(Transformers.AliasToBean(typeof(ConfiguracaoTerminalNfce)));

            var resultado = query.UniqueResult<ConfiguracaoTerminalNfce>();

            return resultado?.IntervaloSync ?? 0;
        }

        public void Deletar(ConfiguracaoTerminalNfce configuracaoLocalNfce)
        {
            _sessao.Delete(configuracaoLocalNfce);
        }

        public byte ObtemIdentificadorTerminal()
        {
            var query = _sessao.QueryOver<ConfiguracaoTerminalNfce>()
                .Select(i => i.TerminalOfflineId);

            return query.FutureValue<byte>().Value;
        }

        public IList<NfceEmissorFiscal> BuscarTodosEmissorDoTerminal(byte terminalOfflineId)
        {
            return _sessao.QueryOver<NfceEmissorFiscal>().Where(x => x.TerminalOfflineId == terminalOfflineId)
                .List<NfceEmissorFiscal>();
        }
    }
}