using FusionCore.FusionAdm.Sessao;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Infraestrutura;

namespace FusionCore.Vendas.Servicos
{
    public static class ContingenciaAtiva
    {
        public static bool EstaAtiva()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var todasContingencias = new TodasContingencias(sessao);

                if (todasContingencias.ExisteContingenciaEmAberto()) return true;
            }

            return false;
        }
    }
}