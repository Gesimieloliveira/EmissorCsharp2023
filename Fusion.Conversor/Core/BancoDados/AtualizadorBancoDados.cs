using FusionCore.FusionAdm.Sessao;
using FusionCore.MigracaoFluente;

namespace Fusion.Conversor.Core.BancoDados
{
    public class AtualizadorBancoDados
    {
        private readonly IMigracao _migrador;

        public AtualizadorBancoDados()
        {
            _migrador = MigracaoFactory.CriaMigrador(SessaoHelperFactory.GetConexaoCfg(), MigracaoTag.Adm);
        }

        public bool PrecisaAtualizar()
        {
            return _migrador.PrecisaAtualizar;
        }

        public void Atualizar()
        {
            _migrador.Migracao();
        }
    }
}