using FusionCore.FusionNfce.Sessao.Sistema;

namespace FusionCore.FusionAdm.Acbr.Servicos
{
    public static class AbrirGaveta
    {
        public static void Executar()
        {
            if (SessaoSistemaNfce.ImpressaoDireta.Desativa) return;
            
            ComandoAbrirGaveta();            
        }

        private static void ComandoAbrirGaveta()
        {
            using (var acbr = new AcbrMonitorPlus())
            {
                acbr.EnviarComando(AcbrMonitorPlusComando.ESCPOS_Ativar);
                acbr.EnviarComando(AcbrMonitorPlusComando.ESCPOS_ImprimirLinha, "</abre_gaveta>");
                acbr.EnviarComando(AcbrMonitorPlusComando.ESCPOS_Desativar);
            }
        }
    }
}