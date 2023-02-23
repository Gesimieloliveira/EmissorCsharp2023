using System;
using DFe.Classes.Flags;
using ModeloDocumentoFusion = FusionCore.FusionAdm.Fiscal.Flags.ModeloDocumento;


namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class ModeloDocumentoExt
    {
        public static ModeloDocumento ToZeus(this ModeloDocumentoFusion modelo)
        {
            switch (modelo)
            {
                case ModeloDocumentoFusion.NFe:
                    return ModeloDocumento.NFe;
                case ModeloDocumentoFusion.NFCe:
                    return ModeloDocumento.NFCe;
                case ModeloDocumentoFusion.MDFe:
                    return ModeloDocumento.MDFe;
            }

            throw new InvalidCastException("ModeloDocumento para zeus é inválido");
        }
    }
}