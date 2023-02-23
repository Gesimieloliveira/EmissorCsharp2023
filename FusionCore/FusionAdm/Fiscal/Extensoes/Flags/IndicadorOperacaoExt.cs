using FusionCore.FusionAdm.Fiscal.Flags;
using ConsumidorFinalZeus = NFe.Classes.Informacoes.Identificacao.Tipos.ConsumidorFinal;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class IndicadorOperacaoExt
    {
        public static ConsumidorFinalZeus ToZeus(this IndicadorOperacaoFinal indicador)
        {
            return indicador == IndicadorOperacaoFinal.Normal
                ? ConsumidorFinalZeus.cfNao
                : ConsumidorFinalZeus.cfConsumidorFinal;
        }
    }
}