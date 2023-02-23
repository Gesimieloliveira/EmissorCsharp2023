using System.Collections.Generic;
using FusionCore.Tributacoes.Flags;

namespace Fusion.Visao.CteEletronicoOs.Emitir.OpcoesTributacao
{
    public static class OpcoesTributacaoFactory
    {
        public static IEnumerable<OpcaoTributacao> CriarOpcoes(RegimeTributario regimeTributario)
        {
            if (regimeTributario == RegimeTributario.SimplesNacional)
                return new List<OpcaoTributacao>()
                {
                    new OpcaoTributacao("ICMS Simples Nacional", "90", false, false, false)
                };

            return new List<OpcaoTributacao>
            {
                new OpcaoTributacao("ICMS Normal", "00", true, false, false),
                new OpcaoTributacao("ICMS Isenção", "40", false, false, false),
                new OpcaoTributacao("ICMS Não Tributado", "41", false, false, false),
                new OpcaoTributacao("ICMS Diferido", "51", false, false, false),
                new OpcaoTributacao("ICMS Outros", "90", true, true, true),
            };
        }
    }
}