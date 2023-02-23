using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoTomador
    {
        public static FusionTipoTomadorCTe ToXml(this TipoTomador tipoTomador)
        {
            switch (tipoTomador)
            {
                case TipoTomador.Destinatario:
                    return FusionTipoTomadorCTe.Destinatario;
                case TipoTomador.Expedidor:
                    return FusionTipoTomadorCTe.Expedidor;
                case TipoTomador.Recebedor:
                    return FusionTipoTomadorCTe.Recebedor;
                case TipoTomador.Remetente:
                    return FusionTipoTomadorCTe.Remetente;
            }

            throw new ArgumentException("Tipo tomador inválido");
        }
    }
}