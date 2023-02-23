using System;
using System.Text;

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    public static class TipoLicencaFactory
    {
        public static TipoLicenca CriaApartirString(string modulo)
        {
            return CriaApartirBytes(Encoding.ASCII.GetBytes(modulo));
        }

        public static TipoLicenca CriaApartirBytes(byte[] bytes)
        {
            var moduloString = Encoding.ASCII.GetString(bytes);

            switch (moduloString)
            {
                case "FS":
                    return TipoLicenca.FusionStarter;
                case "FG":
                    return TipoLicenca.FusionGestor;
                case "FC":
                    return TipoLicenca.FusionCTe;
                case "AA":
                    return TipoLicenca.FusionAdicional;
                case "CR":
                    return TipoLicenca.ChaveRevalidacao;
                case "AR":
                    return TipoLicenca.AutorizacaoRevalidacao;
                case "FM":
                    return TipoLicenca.FusionMDFe;
                case "CO":
                    return TipoLicenca.FusionCTeOS;
            }

            throw new InvalidOperationException("Tipo de licença não suportado mais pelo Fusion");
        }
    }
}