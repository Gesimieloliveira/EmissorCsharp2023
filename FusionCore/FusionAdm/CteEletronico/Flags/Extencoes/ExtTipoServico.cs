using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoServico
    {
        public static FusionTipoServicoCTe ToXml(this TipoServico tipoServico)
        {
            switch (tipoServico)
            {
                case TipoServico.Normal:
                    return FusionTipoServicoCTe.Normal;
                case TipoServico.Subcontratacao:
                    return FusionTipoServicoCTe.Subcontratacao;
            }

            throw new ArgumentException("Tipo serviço inválido");
        }
    }
}