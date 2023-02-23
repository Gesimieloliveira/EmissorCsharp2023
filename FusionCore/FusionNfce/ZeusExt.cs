using NFe.Classes.Servicos.Consulta;

namespace FusionCore.FusionNfce
{
    public static class ZeusExt
    {
        public static bool IsAutorizado(this retConsSitNFe consSitNFe)
        {
            var codigoStatus = consSitNFe.cStat;
            return codigoStatus == 100 || codigoStatus == 150 || codigoStatus == 110;
        }

        public static bool NaoConstaSefaz(this retConsSitNFe consSitNFe)
        {
            var codigoStatus = consSitNFe.cStat;
            return codigoStatus == 217;
        }
    }
}