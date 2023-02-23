using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtModeloNotaFiscal
    {
        public static FusionModeloNotaFiscalCTe ToXml(this ModeloNotaFiscal modeloNotaFiscal)
        {
            switch (modeloNotaFiscal)
            {
                case ModeloNotaFiscal.NFModelo011AeAvulsa:
                    return FusionModeloNotaFiscalCTe.NFModelo011AeAvulsa;
                case ModeloNotaFiscal.NFdeProdutor:
                    return FusionModeloNotaFiscalCTe.NFdeProdutor;
            }

            throw new InvalidOperationException("Modelo nota fiscal inválido");
        }
    }
}