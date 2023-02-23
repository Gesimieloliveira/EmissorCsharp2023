using System;
using FusionCore.DFe.XmlCte;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.CteEletronico.Extencoes
{
    public static class ExtEstadoDTO
    {
        public static FusionEstadoUFCTe ToXml(this EstadoDTO estado)
        {
            switch (estado.Sigla)
            {
                case "AC":
                    return FusionEstadoUFCTe.AC;
                case "AL":
                    return FusionEstadoUFCTe.AL;
                case "AP":
                    return FusionEstadoUFCTe.AP;
                case "AM":
                    return FusionEstadoUFCTe.AM;
                case "BA":
                    return FusionEstadoUFCTe.BA;
                case "CE":
                    return FusionEstadoUFCTe.CE;
                case "DF":
                    return FusionEstadoUFCTe.DF;
                case "ES":
                    return FusionEstadoUFCTe.ES;
                case "GO":
                    return FusionEstadoUFCTe.GO;
                case "MA":
                    return FusionEstadoUFCTe.MA;
                case "MT":
                    return FusionEstadoUFCTe.MT;
                case "MS":
                    return FusionEstadoUFCTe.MS;
                case "MG":
                    return FusionEstadoUFCTe.MG;
                case "PA":
                    return FusionEstadoUFCTe.PA;
                case "PB":
                    return FusionEstadoUFCTe.PB;
                case "PR":
                    return FusionEstadoUFCTe.PR;
                case "PE":
                    return FusionEstadoUFCTe.PE;
                case "PI":
                    return FusionEstadoUFCTe.PI;
                case "RJ":
                    return FusionEstadoUFCTe.RJ;
                case "RN":
                    return FusionEstadoUFCTe.RN;
                case "RS":
                    return FusionEstadoUFCTe.RS;
                case "RO":
                    return FusionEstadoUFCTe.RO;
                case "RR":
                    return FusionEstadoUFCTe.RR;
                case "SC":
                    return FusionEstadoUFCTe.SC;
                case "SP":
                    return FusionEstadoUFCTe.SP;
                case "SE":
                    return FusionEstadoUFCTe.SE;
                case "TO":
                    return FusionEstadoUFCTe.TO;
            }

            throw new ArgumentException("Estado UF inválido");
        }
    }
}