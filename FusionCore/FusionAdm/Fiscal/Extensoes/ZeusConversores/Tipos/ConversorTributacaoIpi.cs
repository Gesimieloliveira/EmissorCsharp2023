using System;
using FusionCore.Tributacoes.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos
{
    public static class ConversorTributacaoIpi
    {
        private static readonly Array ZeusTipos;

        static ConversorTributacaoIpi()
        {
            ZeusTipos = Enum.GetValues(typeof(CSTIPI));
        }

        public static CSTIPI ToZeus(this TributacaoIpi ipi)
        {
            foreach (var zeusTipo in ZeusTipos)
            {
                var padraoComparar = $"ipi{ipi.Codigo}";
                if (zeusTipo.ToString() == padraoComparar)
                    return (CSTIPI) zeusTipo;
            }

            throw new InvalidCastException("Não foi possível converter o IPI para um IPI do Zeus");
        }
    }
}
