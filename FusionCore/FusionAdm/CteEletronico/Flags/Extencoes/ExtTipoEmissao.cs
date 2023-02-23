using System;
using DFe.Classes.Entidades;
using DFe.Classes.Extensoes;
using FusionCore.DFe.XmlCte;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoEmissao
    {
        public static FusionTipoEmissaoCTe ToXml(this TipoEmissao tipoEmissao, EstadoDTO estadoDTO)
        {
            switch (tipoEmissao)
            {
                case TipoEmissao.Normal:
                    return FusionTipoEmissaoCTe.Normal;
                case TipoEmissao.Contingencia:
                    return ObterServidorContingencia(estadoDTO);
                default:
                    throw new ArgumentException("Tipo emissão inválido (CT-e)");
            }
        }

        private static FusionTipoEmissaoCTe ObterServidorContingencia(EstadoDTO estadoDTO)
        {
            var estado = Estado.AC;

            estado = estado.CodigoIbgeParaEstado(estadoDTO.CodigoIbge.ToString());

            switch (estado)
            {
                case Estado.AC:
                case Estado.AL:
                case Estado.AM:
                case Estado.BA:
                case Estado.CE:
                case Estado.DF:
                case Estado.ES:
                case Estado.GO:
                case Estado.MA:
                case Estado.PA:
                case Estado.PB:
                case Estado.PI:
                case Estado.RJ:
                case Estado.RN:
                case Estado.RO:
                case Estado.SC:
                case Estado.SE:
                case Estado.TO:
                case Estado.RS:
                case Estado.MG:
                case Estado.PR:
                    return FusionTipoEmissaoCTe.AutorizacaoSvcSp;

                case Estado.SP:
                case Estado.RR:
                case Estado.PE:
                case Estado.AP:
                case Estado.MT:
                case Estado.MS:
                case Estado.AN:
                    return FusionTipoEmissaoCTe.AutorizacaoSvcRs;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}