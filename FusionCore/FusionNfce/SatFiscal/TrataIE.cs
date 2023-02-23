using System;
using FusionCore.FusionNfce.Sessao.Sistema;

namespace FusionCore.FusionNfce.SatFiscal
{
    public static class TrataIE
    {
        public static string TrataXmlIE(string xml)
        {
            var inscricaoEstadual = SessaoSistemaNfce.Empresa().InscricaoEstadual;

            if (SessaoSistemaNfce.IsMFe())
                return xml.Replace("<IE>" + inscricaoEstadual, "<IE>" + inscricaoEstadual.PadLeft(12, '0'));
            if (SessaoSistemaNfce.IsEmissorSat())
                return xml.Replace("<IE>" + inscricaoEstadual, "<IE>" + inscricaoEstadual.PadRight(12, ' '));

            throw new InvalidOperationException();
        }
    }
}