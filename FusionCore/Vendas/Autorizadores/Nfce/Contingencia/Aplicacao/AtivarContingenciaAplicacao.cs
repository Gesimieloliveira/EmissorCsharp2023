using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio;

namespace FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Aplicacao
{
    public class AtivarContingenciaAplicacao : IAtivarContingenciaAplicacao
    {
        private readonly ITodasContingenciasNfce _todasContingencias;
        private readonly IAtivarContingenciaDominio _ativarContingenciaDominio;

        public AtivarContingenciaAplicacao(IAtivarContingenciaDominio ativarContingenciaDominio, ITodasContingenciasNfce todasContingencias)
        {
            _todasContingencias = todasContingencias;
            _ativarContingenciaDominio = ativarContingenciaDominio;
        }

        public void Ativar()
        {
            if (_todasContingencias.ExisteContingenciaEmAberto()) return;

            var contingencia = _ativarContingenciaDominio.Ativar();

            _todasContingencias.Salvar(contingencia);
        }
    }
}