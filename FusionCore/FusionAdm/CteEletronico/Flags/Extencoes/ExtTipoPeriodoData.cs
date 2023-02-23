using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoPeriodoData
    {
        public static FusionTipoPeriodoDataCTe ToXml(this TipoPeriodoData periodoData)
        {
            switch (periodoData)
            {
                case TipoPeriodoData.SemDataDefinida:
                    return FusionTipoPeriodoDataCTe.SemDataDefinida;
                case TipoPeriodoData.NaData:
                    return FusionTipoPeriodoDataCTe.NaData;
                case TipoPeriodoData.AteAData:
                    return FusionTipoPeriodoDataCTe.AteAData;
                case TipoPeriodoData.APartirDaData:
                    return FusionTipoPeriodoDataCTe.APartirDaData;
                case TipoPeriodoData.NoPeriodo:
                    return FusionTipoPeriodoDataCTe.NoPeriodo;
            }

            throw new InvalidOperationException("Tipo periodo data inválida");
        }
    }
}