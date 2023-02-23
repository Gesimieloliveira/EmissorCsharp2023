using System;
using DFe.Classes.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeModeloManifesto
    {
        public static ModeloDocumento ToZeusMdfe(this MDFeModeloManifesto modeloManifesto)
        {
            switch (modeloManifesto)
            {
                    case MDFeModeloManifesto.MDFe:
                    return ModeloDocumento.MDFe;
            }

            throw new InvalidOperationException("Modelo manifesto inválido");
        }
    }
}