using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoPeriodoHora
    {
        public static FusionTipoPeriodoHoraCTe ToXml(this TipoPeriodoHora periodoHora)
        {
            switch (periodoHora)
            {
                case TipoPeriodoHora.SemHoraDefinida:
                    return FusionTipoPeriodoHoraCTe.SemHoraDefinida;
                case TipoPeriodoHora.NoHorario:
                    return FusionTipoPeriodoHoraCTe.NoHorario;
                case TipoPeriodoHora.AteOHorario:
                    return FusionTipoPeriodoHoraCTe.AteOHorario;
                case TipoPeriodoHora.APartirDoHorario:
                    return FusionTipoPeriodoHoraCTe.APartirDoHorario;
                case TipoPeriodoHora.NoIntervaloDeTempo:
                    return FusionTipoPeriodoHoraCTe.NoIntervaloDeTempo;
            }

            throw new InvalidOperationException("Tipo periodo hora inválido");
        }
    }
}