using System;
using System.Linq;
using NHibernate;

namespace FusionCore.MigracaoFluente
{
    public class ChecarVersaoBdAdm
    {
        private readonly IMigracao _migracao;
        private readonly ISession _sessao;

        public ChecarVersaoBdAdm(IMigracao migracao, ISession sessao)
        {
            _migracao = migracao;
            _sessao = sessao;
        }

        public void Checar()
        {
            var ultimaVersao = _sessao.QueryOver<VersaoInformacao>().OrderBy(v => v.VersaoAplicada).Desc
                .List<VersaoInformacao>().FirstOrDefault();

            if (ultimaVersao == null) return;

            if (ultimaVersao.VersaoAplicada != _migracao.UltimaVersaoInterna)
                throw new InvalidOperationException("Atualize o Sistema Fusion nesse terminal");
        }
    }
}